using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;

public class PlacedTile : MonoBehaviour
{
    public enum PlacedTileType
    {
        Normal,
        Light,
        ColorSetter,
        Teleport,
        Lifter,
    }

    [Header("Settings")] 
    [SerializeField] private PlacedTileType _type = PlacedTileType.Normal;
    [SerializeField] private float _startHeight = 0;

    [Header("References")]
    [SerializeField] private SpriteRenderer _topSpriteRenderer;
    [SerializeField] private PlacedTile _teleportDestinationTile = null;

    [SerializeField] private Transform _lifterTopTransform;
    [SerializeField] private Transform _lifterBarTransform;

    [Header("Prefabs")] 
    [SerializeField] private CurvedLineRenderer _curvedLineRendererPrefab;
    
    [Header("Sprites")]
    [SerializeField] private Sprite _lightOffSprite;
    [SerializeField] private Sprite _lightOnSprite;
    [SerializeField] private Sprite _noLightSprite;
    [SerializeField] private Sprite _conditionalSprite;
    [SerializeField] private Sprite _teleportIndicationSprite;
    
    [Header("Variables")]
    [SerializeField] private FloatVariable _startYPosition;
    [SerializeField] private FloatVariable _animationDuration;
    [SerializeField] private FloatVariable _cellSize;
    [SerializeField] private ColorVariable _color;
    [SerializeField] private FloatVariable _lifterMoveDuration;
    
    [Header("Events")]
    [SerializeField] private GameEvent _resetLevelGameEvent;
    
    [Header("Runtime Sets")]
    [SerializeField] private PlacedTileRuntimeSet _lightTilesRuntimeSet;
    [SerializeField] private PlayerControllerRuntimeSet _playerControllerRuntimeSet;
    
    [HideInInspector] public CurvedLineRenderer curvedLineRenderer;
    
    public PlacedTileType type => _type;

    private float _currentHeight;
    private Vector3 _endPosition;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
        _endPosition = transform.position;
        _transform.position = new Vector3(_endPosition.x, _startYPosition.Value, _endPosition.z);
    }

    private void OnEnable()
    {
        _resetLevelGameEvent.AddListener(TurnLightOff);
        _resetLevelGameEvent.AddListener(ResetLifterHeight);
        if (_type == PlacedTileType.Light)
        {
            _lightTilesRuntimeSet.AddToList(this);
        }
    }

    private void ResetLifterHeight()
    {
        if (_type == PlacedTileType.Lifter)
        {
            _currentHeight = _startHeight;
            _lifterBarTransform.DOScaleY(_currentHeight, 0.01f);
            _lifterTopTransform.DOLocalMoveY(_currentHeight + 0.5f, 0.01f);
        }
    }

    private void Start()
    {
        _currentHeight = _startHeight;
        this.Wait(0.1f, Initialize); // Wait for the level manager to be updated
    }

    private void Initialize()
    {
        if (_type == PlacedTileType.Light)
        {
            _topSpriteRenderer.sprite = _lightOffSprite;
        }
        else if (_type == PlacedTileType.ColorSetter)
        {
            _topSpriteRenderer.sprite = _conditionalSprite;
        }
        else if (_type == PlacedTileType.Teleport)
        {
            _topSpriteRenderer.sprite = _teleportIndicationSprite;
            CreateTeleportLineRenderer(); 
        }
        else
        {
            _topSpriteRenderer.sprite = _noLightSprite;
        }
    }

    private void CreateTeleportLineRenderer()
    {
        if (_teleportDestinationTile != null)
        {
            if (_curvedLineRendererPrefab != null && curvedLineRenderer == null)
            {
                curvedLineRenderer = Instantiate(_curvedLineRendererPrefab, transform);
                //The level manager must be updated to get the current position of the tile
                Vector3? currentTilePosition = LevelManager.instance.GetCurrentTilePosition(_endPosition); 
                if (currentTilePosition.HasValue)
                {
                    curvedLineRenderer.SetStartPoint(currentTilePosition.Value + new Vector3(0, _startYPosition.Value, 0), transform); 
                    Vector3? currentTeleportDestinationPosition = LevelManager.instance.GetCurrentTilePosition(_teleportDestinationTile.transform.position);
                    if (currentTeleportDestinationPosition.HasValue)
                    {
                        curvedLineRenderer.SetEndPoint(currentTeleportDestinationPosition.Value + new Vector3(0, _startYPosition.Value, 0), _teleportDestinationTile.transform);
                        _teleportDestinationTile.SetTeleportDestinationPlacedTile(this);
                        if (currentTeleportDestinationPosition.Value.x + currentTeleportDestinationPosition.Value.z < currentTilePosition.Value.x + currentTilePosition.Value.z)
                        {
                            curvedLineRenderer.SetMiddlePointParent(_teleportDestinationTile.transform);
                        }
                        else
                        {
                            curvedLineRenderer.SetMiddlePointParent(transform);
                        }
                        _teleportDestinationTile.curvedLineRenderer = curvedLineRenderer;
                    }
                }
                else
                {
                    Debug.Log("Current tile position is null!");
                }
            }
        }
    }
    
    public void HideTile()
    {
        _transform.position = new Vector3(_endPosition.x, _startYPosition.Value, _endPosition.z);
    }

    public void HideTile(float delay)
    {
        this.Wait(delay, AnimateTileToStartPosition);
    }
    
    public void ShowTile(float delay)
    {
        // Reset start position
        HideTile();
        // Wait before execution
        this.Wait(delay, AnimateTileToEndPosition);
    }

    private void AnimateTileToEndPosition()
    {
        _transform.DOMoveY(_endPosition.y, _animationDuration.Value).SetEase(Ease.OutBack);
    }

    private void AnimateTileToStartPosition()
    {
        _transform.DOMoveY(_startYPosition, _animationDuration.Value).SetEase(Ease.InBack);
    }
    
    public void TurnLightOn()
    {
        if (_type == PlacedTileType.Light)
        {
            _topSpriteRenderer.sprite = _lightOnSprite;
            _lightTilesRuntimeSet.RemoveFromList(this);
        }
        else if (_type == PlacedTileType.Lifter)
        {
            _currentHeight += 1.0f;
            if (_currentHeight > 2.0f)
            {
                _currentHeight = 0;
            }

            PlayerController playerController = _playerControllerRuntimeSet.GetItemIndex(0);
            if (playerController != null)
            {
                playerController.transform.DOMoveY(_currentHeight + 1.0f, _lifterMoveDuration.Value).OnComplete(() =>
                {
                    playerController.SetCurrentHeight(_currentHeight);
                });
            }
            _lifterBarTransform.DOScaleY(_currentHeight, _lifterMoveDuration.Value);
            _lifterTopTransform.DOLocalMoveY(_currentHeight + 0.5f, _lifterMoveDuration.Value);
        }
    }
    
    
    
    public void TurnLightOff()
    {
        if (_type == PlacedTileType.Light)
        {
            _topSpriteRenderer.sprite = _lightOffSprite;
            _lightTilesRuntimeSet.AddToList(this);
        }
    }
    
    public float GetHeight()
    {
        return _startHeight;
    }

    private void OnDisable()
    {
        _lightTilesRuntimeSet.RemoveFromList(this);
        _resetLevelGameEvent.RemoveListener(TurnLightOff);
        _resetLevelGameEvent.RemoveListener(ResetLifterHeight);
    }

    public Vector3 GetTileTopCenterWorldPosition()
    {
        return new Vector3(transform.position.x + (_cellSize.Value / 2), _startHeight + 1, transform.position.z + (_cellSize.Value / 2));
    }

    public Vector3 GetTileTopCenterLocalPosition()
    {
        return new Vector3((_cellSize.Value / 2), _startHeight + 1, (_cellSize.Value / 2));
    }
    
    public override string ToString()
    {
        return name;
    }

    public ColorVariable GetColor()
    {
        return _color;
    }

    public void SetTeleportDestinationPlacedTile(PlacedTile placedTile)
    {
        if (_type != PlacedTileType.Teleport)
        {
            Debug.LogError("Trying to set teleport destination placed tile but " + name +
                           " is not set as teleport type!");
        }

        if (placedTile.type != PlacedTileType.Teleport)
        {
            Debug.LogError("Trying to set " + placedTile.name + " as teleport destination of " + name + " but " +
                           placedTile.name + " is not set as teleport type!");
        }
        
        _teleportDestinationTile = placedTile;
    }

    public PlacedTile GetTeleportDestination()
    {
        if (_type != PlacedTileType.Teleport)
        {
            Debug.LogError("Trying to get teleport position but " + name + " is not set as teleport type!");
            return null;
        }
        
        return _teleportDestinationTile;
    }
    
    public Vector3? GetTeleportPosition()
    {
        if (_type != PlacedTileType.Teleport)
        {
            Debug.LogError("Trying to get teleport position but " + name + " is not set as teleport type!");
            return null;
        }
        
        if (_teleportDestinationTile != null)
        {
            return LevelManager.instance.GetCurrentTilePosition(_teleportDestinationTile.transform.position);
        }
        
        Debug.LogError(name + "is set as teleport, but teleport destination is null!");

        return null;
    }
}
