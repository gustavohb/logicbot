using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;

public class PlacedTile : MonoBehaviour
{
    [SerializeField] private bool _isLightTile = false;
    [SerializeField] private float _height = 0;

    [SerializeField] private SpriteRenderer _topSpriteRenderer;

    [SerializeField] private Sprite _lightOffSprite;
    [SerializeField] private Sprite _lightOnSprite;
    [SerializeField] private Sprite _noLightSprite;

    [SerializeField] private GameEvent _resetLevelGameEvent;

    [SerializeField] private FloatVariable _startYPosition;
    [SerializeField] private FloatVariable _animationDuration;

    [SerializeField] private FloatVariable _cellSize;

    [SerializeField] private PlacedTileRuntimeSet _lightTilesRuntimeSet;
    
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
        if (_isLightTile)
        {
            _lightTilesRuntimeSet.AddToList(this);
        }
    }

    private void Start()
    {
        if (_isLightTile)
        {
            _topSpriteRenderer.sprite = _lightOffSprite;
        }
        else
        {
            _topSpriteRenderer.sprite = _noLightSprite;
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
    
    
    public bool isLightTile
    {
        get => _isLightTile;
    }

    public void TurnLightOn()
    {
        if (_isLightTile)
        {
            _topSpriteRenderer.sprite = _lightOnSprite;
            _lightTilesRuntimeSet.RemoveFromList(this);
        }
    }

    public void TurnLightOff()
    {
        if (_isLightTile)
        {
            _topSpriteRenderer.sprite = _lightOffSprite;
            _lightTilesRuntimeSet.AddToList(this);
        }
    }
    
    public float GetHeight()
    {
        return _height;
    }

    private void OnDisable()
    {
        _lightTilesRuntimeSet.RemoveFromList(this);
        _resetLevelGameEvent.RemoveListener(TurnLightOff);
    }

    public Vector3 GetTileTopCenterWorldPosition()
    {
        return new Vector3(transform.position.x + (_cellSize.Value / 2), _height + 1, transform.position.z + (_cellSize.Value / 2));
    }

    public Vector3 GetTileTopCenterLocalPosition()
    {
        return new Vector3((_cellSize.Value / 2), _height + 1, (_cellSize.Value / 2));
    }
    
    public override string ToString()
    {
        return name;
    }
}
