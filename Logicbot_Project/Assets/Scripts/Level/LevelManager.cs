using System;
using UnityEngine;
using ScriptableObjectArchitecture;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Grid Settings")]
    [SerializeField] private int _gridWidth = 10;
    [SerializeField] private int _gridHeight = 10;
    [SerializeField] private FloatVariable _cellSize;
    [SerializeField] private Transform _gridOriginTransform;
    [SerializeField] private bool _showDebugLines = true;
    
    [Header("References")]
    [SerializeField] private PlacedTileRuntimeSet _placedTileRuntimeSet;
    [SerializeField] private PlacedTileRuntimeSet _lightTilesRuntimeSet;
    [SerializeField] private LevelRepositorySO _levelRepository;    
    [SerializeField] private Transform _playerPrefab;

    [Header("Variables")] 
    [SerializeField] private BoolVariable _isShowingTiles;
    [SerializeField] private BoolVariable _isHidingTiles;
    [SerializeField] private BoolVariable _isLoadingLevel;
    [SerializeField] private BoolVariable _isLevelCompleted;
    [SerializeField] private BoolVariable _stopped;
    [SerializeField] private IntVariable _currentLevelIndex;
    [SerializeField] private IntVariable _totalLevels;
    
    [Header("Events")]
    [SerializeField] private GameEvent _resetLevelGameEvent;
    [SerializeField] private GameEvent _reloadLevelGameEvent;
    [SerializeField] private LevelDataGameEvent _setCurrentLevelDataGameEvent;
    [SerializeField] private GameEvent _levelCompletedGameEvent;
    [SerializeField] private GameEvent _loadNextLevelGameEvent;
    [SerializeField] private IntGameEvent _loadLevelGameEvent;
    
    [Header("SFX")]
    [SerializeField] private AudioCueEventChannelSO _sfxEventChannel = default;
    [SerializeField] private AudioConfigurationSO _audioConfig = default;

    [SerializeField] private AudioCueSO _showTilesSFX;
    [SerializeField] private AudioCueSO _hideTilesSFX;
    [SerializeField] private AudioCueSO _levelCompletedSFX;
    
    
    private PlacedTile _playerStartTile;
    private PlayerController.Dir _playerStartDir;
    private LevelDataSO _currentLevelData;
    private Transform _playerTransform;
    
    private GridXZ<GridObject> _grid;

    private LevelTiles _currentLevelTiles;

    private float _delayFactor = 0.075f;
    
    protected override void Awake()
    {
        base.Awake();
        _grid = new GridXZ<GridObject>(_gridWidth, _gridHeight, _cellSize.Value,
            _gridOriginTransform != null ? _gridOriginTransform.position : Vector3.zero,
            _showDebugLines,
            (GridXZ<GridObject> g, int x, int y) => new GridObject(g, x, y));
        
        _placedTileRuntimeSet.onRuntimeSetChanged += PlacedTileRuntimeSetOnRuntimeSetChanged;

        _setCurrentLevelDataGameEvent.AddListener(SetCurrentLevelData);
        
        _totalLevels.Value = _levelRepository.levelList.Count;
        
        _isHidingTiles.Value = false;
        _isShowingTiles.Value = false;
        _isLoadingLevel.Value = false;
        _isLevelCompleted.Value = false;
        _currentLevelIndex.Value = -1;
    }

    private void Start()
    {
        _stopped.Value = false;
        _stopped.Raise();
    }

    private void SetCurrentLevelData(LevelDataSO levelData)
    {
        _currentLevelData = levelData;
        _playerStartDir = levelData.playerStartDir; //TODO: Move to appropriated place
        this.Wait(1f, InstantiateLevelTiles); //TODO: Move to appropriated place
        this.Wait(1.5f, ShowGrid); //TODO: Move to appropriated place
    }

    private void CheckIfLevelIsCompleted()
    {
        if (_lightTilesRuntimeSet.Count() == 0 && !_isLoadingLevel.Value)
        {
            PlayAudio(_levelCompletedSFX, _audioConfig, Vector3.zero);
            _isLevelCompleted.Value = true;
            _stopped.Value = true;
            _levelCompletedGameEvent.Raise();
        }
    }
    
    private void OnEnable()
    {
        _reloadLevelGameEvent.AddListener(ReloadLevel);
        _loadNextLevelGameEvent.AddListener(LoadNextLevel);
        _loadLevelGameEvent.AddListener(LoadLevel);
        _lightTilesRuntimeSet.onRuntimeSetChanged += LightTilesRuntimeSet_OnRuntimeSetChanged;
    }

    private void LightTilesRuntimeSet_OnRuntimeSetChanged(object sender, BaseRuntimeSet<PlacedTile>.RuntimeSetEventArgs<PlacedTile> e)
    {
        CheckIfLevelIsCompleted();
    }

    private void RaiseSetCurrentLevelDataGameEvent()
    {
        _setCurrentLevelDataGameEvent.Raise(_currentLevelData);
    }
    
    private void ReloadLevel()
    {
        if (_isLoadingLevel.Value || !_stopped.Value)
        {
            return;
        }

        _stopped.Value = true;
        _resetLevelGameEvent.Raise();
        _isLoadingLevel.Value = true;
        ShowGrid();
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < _levelRepository.levelList.Count)
        {
            _isLoadingLevel.Value = true;
            _currentLevelIndex.Value = levelIndex;
            HideTiles(() =>
            {
                if (_currentLevelIndex.Value >= 0 && _currentLevelIndex.Value < _levelRepository.levelList.Count)
                {
                    _currentLevelData = _levelRepository.levelList[_currentLevelIndex.Value];
                    RaiseSetCurrentLevelDataGameEvent();    
                }
            });
        }
    }

    public void LoadNextLevel()
    {
        _isLevelCompleted.Value = false;
        _currentLevelIndex.Value++;
        HideTiles(() =>
        {
            if (_currentLevelIndex.Value >= 0 && _currentLevelIndex.Value < _levelRepository.levelList.Count)
            {
                _currentLevelData = _levelRepository.levelList[_currentLevelIndex.Value];
                RaiseSetCurrentLevelDataGameEvent();
                ShowGrid();
            }
        });
    }


    public void ShowGrid()
    {
        if (_isShowingTiles.Value || _isHidingTiles.Value)
        {
            return;
        }

        _isShowingTiles.Value = true;
        
        ResetPlayerPosition();

        int gridCols = _grid.GetWidth();
        int gridRows = _grid.GetHeight();
        
        int startCol = 0;
        int startRow = 0;

        int maxDist = 0;
        
        PlacedTile currentPlacedTile = null;
        
        for (int col = 0; col < gridCols; col++) {
            for (int row = 0; row < gridRows; row++)
            {
                int dist = Mathf.Abs(startCol - col) + Mathf.Abs(startRow - row); // calculate distance
                if (maxDist < dist)
                {
                    maxDist = dist;
                }
                
                currentPlacedTile = _grid[row, col].GetPlacedTile();
                if (currentPlacedTile != null)
                {
                    currentPlacedTile.ShowTile(_delayFactor * dist);
                }
            }
        }

        this.Wait(0.3f, PlayShowTilesSound);
        
        this.Wait(_delayFactor * maxDist + 1f, () =>
        {
            _isShowingTiles.Value = false;
            _isLoadingLevel.Value = false;
            _stopped.Value = true;
        });
    }

    public void HideGridImmediately()
    {
        for (int i = 0; i < _grid.GetWidth(); i++)
        {
            for (int j = 0; j < _grid.GetHeight(); j++)
            {
                PlacedTile placedTile = _grid[i, j].GetPlacedTile();
                if (placedTile != null)
                {
                    placedTile.HideTile();
                }
            }
        }
    }
    
    public void HideTiles(Action callback)
    {
        if (_isHidingTiles.Value || _isShowingTiles.Value)
        {
            return;
        }

        if (_placedTileRuntimeSet.Count() == 0)
        {
            callback?.Invoke();
            return;
        }

        _isHidingTiles.Value = true;

        if (_playerTransform != null)
        {
            GridObject gridObject = _grid.GetGridObject(_playerTransform.position);
            if (gridObject != null)
            {
                PlacedTile placedTile = gridObject.GetPlacedTile();
                if (placedTile != null)
                {
                    _playerTransform.parent = placedTile.transform;
                }
            }
        }
        
        int gridCols = _grid.GetWidth();
        int gridRows = _grid.GetHeight();
        
        int startCol = gridCols;
        int startRow = gridRows;

        int maxDist = 0;
        
        PlacedTile currentPlacedTile = null;
        
        for (int col = 0; col < gridCols; col++) {
            for (int row = 0; row < gridRows; row++)
            {
                int dist = Mathf.Abs(startCol - col) + Mathf.Abs(startRow - row); // calculate distance
                if (maxDist < dist)
                {
                    maxDist = dist;
                }
                
                currentPlacedTile = _grid[row, col].GetPlacedTile();
                if (currentPlacedTile != null)
                {
                    currentPlacedTile.HideTile(_delayFactor * dist);
                }
            }
        }

        this.Wait(0.5f * maxDist * _delayFactor, PlayHideTilesSound);
        
        this.Wait(_delayFactor * maxDist, () =>
        {
            callback?.Invoke();
            _isHidingTiles.Value = false;
        });
    }


    private void InstantiateLevelTiles()
    {
        if (_currentLevelTiles != null)
        {
            Destroy(_currentLevelTiles.gameObject);
            _currentLevelTiles = null;
        }

        if (_playerTransform != null)
        {
            Destroy(_playerTransform.gameObject);
            _playerTransform = null;
        }
        
        _currentLevelTiles = Instantiate(_currentLevelData.levelTiles);
        _playerStartTile = _currentLevelTiles.playerStartTile;

        if (_playerPrefab != null)
        {
            _playerTransform = Instantiate(_playerPrefab, _playerStartTile.transform, true);
        }
        
        ResetPlayerPosition();
    }

    private void ResetPlayerPosition()
    {
        if (_playerTransform != null && _playerStartTile != null)
        {
            _playerTransform.localPosition = _playerStartTile.GetTileTopCenterLocalPosition();
            
            PlayerController playerController = _playerTransform.GetComponent<PlayerController>();
            playerController.SetStartDir(_playerStartDir);
            playerController.SetStartLocalPosition(_playerStartTile.GetTileTopCenterLocalPosition());
            playerController.SetStartHeight(_playerStartTile.GetHeight());
        }
    }
    
    private void PlacedTileRuntimeSetOnRuntimeSetChanged(object sender, BaseRuntimeSet<PlacedTile>.RuntimeSetEventArgs<PlacedTile> e)
    {
        if (!e.removed)
        {
            PlaceTileToTheGrid(e);
        }
        else
        {
            RemoveTileFromTheGrid(e);
        }
    }

    public PlacedTile GetPlacedTileAt(Vector3 worldPosition)
    {
        PlacedTile placedTile = null;
        GridObject gridObject = _grid.GetGridObject(worldPosition);
        if (gridObject != null)
        {
            placedTile = gridObject.GetPlacedTile();
        }

        return placedTile;
    }
    
    private void RemoveTileFromTheGrid(BaseRuntimeSet<PlacedTile>.RuntimeSetEventArgs<PlacedTile> e)
    {
        Vector3 placedTimeWorldPosition = e.obj.transform.position;
        GridObject gridObject = _grid.GetGridObject(placedTimeWorldPosition);
        if (gridObject != null && gridObject.IsOccupied())
        {
            _grid.GetGridObject(placedTimeWorldPosition).ClearPlacedObject();
            _grid.GetXZ(placedTimeWorldPosition, out int x, out int z);
        }
        else
        {
            Debug.LogError("Cannot remove " + e.obj.ToString() + " from the grid!");
        }
    }

    private void PlaceTileToTheGrid(BaseRuntimeSet<PlacedTile>.RuntimeSetEventArgs<PlacedTile> e)
    {
        Vector3 placedTimeWorldPosition = e.obj.transform.position;
        GridObject gridObject = _grid.GetGridObject(placedTimeWorldPosition);
        if (gridObject != null && !gridObject.IsOccupied())
        {
            _grid.GetGridObject(placedTimeWorldPosition).SetPlacedTile(e.obj);
            _grid.GetXZ(placedTimeWorldPosition, out int x, out int z);
        }
        else
        {
            Debug.LogError("Cannot add " + e.obj.ToString() + " to the grid, position is already occupied!");
        }
    }

    public Vector3? GetCurrentTilePosition(Vector3 currentWorldPosition)
    {
        // TODO: Refactor
        _grid.GetXZ(currentWorldPosition, out int x, out int z);
        Vector2Int tileIndex = new Vector2Int(x, z);
        
        GridObject gridObject = _grid.GetGridObject(currentWorldPosition);
        
        if (gridObject != null && gridObject.IsOccupied())
        {
            Vector3? currentPosition = _grid.GetWorldPosition(tileIndex);
            if (currentPosition.HasValue)
            {
                float tileHeight = gridObject.GetPlacedTile().GetHeight();
                return currentPosition.Value + new Vector3(0, tileHeight, 0);
            }
        }
        
        return null;
    }
    
    public Vector3 GetNextPosition(Vector3 currentPosition, PlayerController.Dir currentDirection)
    {   
        //TODO: Refactor
        _grid.GetXZ(currentPosition, out int x, out int z);
        Vector2Int nextTileIndex = new Vector2Int(x, z);
        switch (currentDirection)
        {
            case PlayerController.Dir.Right:
                nextTileIndex += new Vector2Int(1, 0);
                break;
            case PlayerController.Dir.Down:
                nextTileIndex += new Vector2Int(0, -1);
                break;
            case PlayerController.Dir.Left:
                nextTileIndex += new Vector2Int(-1, 0);
                break;
            case PlayerController.Dir.Up:
                nextTileIndex += new Vector2Int(0, 1);
                break;
        }

        Vector3 nextTileWorldPosition = _grid.GetWorldPosition(nextTileIndex.x, nextTileIndex.y);

        GridObject gridObject = _grid.GetGridObject(nextTileWorldPosition);
        
        if (gridObject != null && gridObject.IsOccupied())
        {
            Vector3? nextPosition = _grid.GetWorldPosition(nextTileIndex);

            if (nextPosition.HasValue)
            {
                float tileHeight = gridObject.GetPlacedTile().GetHeight();
                return nextPosition.Value + new Vector3(0, tileHeight, 0);
            }
        }
        
        
        return currentPosition;
    }
    
    private void PlayShowTilesSound() => PlayAudio(_showTilesSFX, _audioConfig, transform.position);
    
    private void PlayHideTilesSound() => PlayAudio(_hideTilesSFX, _audioConfig, transform.position);

    private void PlayAudio(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace = default)
    {
        _sfxEventChannel.RaisePlayEvent(audioCue, audioConfiguration, positionInSpace);
    }
    
    private void OnDisable()
    {
        _reloadLevelGameEvent.RemoveListener(ReloadLevel);
        _loadNextLevelGameEvent.RemoveListener(LoadNextLevel);
        _loadLevelGameEvent.RemoveListener(LoadLevel);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _setCurrentLevelDataGameEvent.RemoveListener(SetCurrentLevelData);
    }
}
