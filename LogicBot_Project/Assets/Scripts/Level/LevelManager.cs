using UnityEngine;
using ScriptableObjectArchitecture;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private int _gridWidth = 10;
    [SerializeField] private int _gridHeight = 10;
    [SerializeField] private FloatVariable _cellSize;
    [SerializeField] private Transform _gridOriginTransform;

    [SerializeField] private GameEvent _resetLevelGameEvent;
    [SerializeField] private GameEvent _reloadLevelGameEvent;
    
    
    [SerializeField] private bool _showDebugLines = true;
    [SerializeField] private PlacedTileRuntimeSet _placedTileRuntimeSet;
    [SerializeField] private Transform _playerPrefab;
    [SerializeField] private LevelDataGameEvent _setCurrentLevelDataGameEvent;
    [SerializeField] private LevelDataSO _startLevelDataTest;
    
    private PlacedTile _playerStartTile;
    private PlayerController.Dir _playerStartDir;
    private LevelDataSO _currentLevelData;
    private Transform _playerTransform;
    
    private GridXZ<GridObject> _grid;

    private LevelTiles _currentLevelTiles;
    
    protected override void Awake()
    {
        base.Awake();
        _grid = new GridXZ<GridObject>(_gridWidth, _gridHeight, _cellSize.Value,
            _gridOriginTransform != null ? _gridOriginTransform.position : Vector3.zero,
            _showDebugLines,
            (GridXZ<GridObject> g, int x, int y) => new GridObject(g, x, y));
        
        _placedTileRuntimeSet.onRuntimeSetChanged += PlacedTileRuntimeSetOnRuntimeSetChanged;

        _setCurrentLevelDataGameEvent.AddListener(SetCurrentLevelData);
    }

    private void SetCurrentLevelData(LevelDataSO levelData)
    {
        _currentLevelData = levelData;
        _playerStartDir = levelData.playerStartDir; //TODO: Move to appropriated place
        InstantiateLevelTiles(); //TODO: Move to appropriated place
        this.Wait(1, ShowGrid); //TODO: Move to appropriated place
    }
    
    private void OnEnable()
    {
        _reloadLevelGameEvent.AddListener(ReloadLevel);
    }

    private void Start()
    {
        //HideGrid();
        _setCurrentLevelDataGameEvent.Raise(_startLevelDataTest);
    }
    
    private void ReloadLevel()
    {
        _resetLevelGameEvent.Raise();
        HideGrid();
        ShowGrid();
    }
    
    
    public void ShowGrid()
    {
        int n = _grid.GetWidth();
        int m = _grid.GetHeight();
        
        int row = 0, col = 0;
        bool up = true;
        
        float delayFactor = 0.025f;

        while (row < m && col < n)
        {
            PlacedTile currentPlacedTile = null;
            if (up)
            {
                while (row > 0 && col < n - 1)
                {
                    currentPlacedTile = _grid[row, col].GetPlacedTile();
                    if (currentPlacedTile != null)
                    {
                        currentPlacedTile.ShowTile(delayFactor * row * col);
                    }
                    row--;
                    col++;
                }
                currentPlacedTile = _grid[row, col].GetPlacedTile();
                if (currentPlacedTile != null)
                {
                    currentPlacedTile.ShowTile(delayFactor * row * col);
                }
                if (col == n - 1)
                {
                    row++;
                }
                else
                {
                    col++;
                }
            }
            else
            {
                while (col > 0 && row < m - 1)
                {
                    currentPlacedTile = _grid[row, col].GetPlacedTile();
                    if (currentPlacedTile != null)
                    {
                        currentPlacedTile.ShowTile(delayFactor * row * col);
                    }
                    row++;
                    col--;
                }
                currentPlacedTile = _grid[row, col].GetPlacedTile();
                if (currentPlacedTile != null)
                {
                    currentPlacedTile.ShowTile(delayFactor * row * col);
                }
                if (row == m - 1)
                {
                    col++;
                }
                else
                {
                    row++;
                }
            }

            up = !up;
        }
    }

    public void HideGrid()
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
            _playerTransform.position = _playerStartTile.GetTileTopCenterWorldPosition();
            PlayerController playerController = _playerTransform.GetComponent<PlayerController>();
            playerController.SetStartDir(_playerStartDir);
            _playerTransform.localPosition = _playerStartTile.GetTileTopCenterLocalPosition();
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
    
    private void OnDisable()
    {
        _reloadLevelGameEvent.RemoveListener(ReloadLevel);
    }

    private void OnDestroy()
    {
        _setCurrentLevelDataGameEvent.RemoveListener(SetCurrentLevelData);
    }
    
}
