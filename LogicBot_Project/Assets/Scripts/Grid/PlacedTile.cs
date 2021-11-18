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
    
    public bool isLightTile
    {
        get => _isLightTile;
    }

    public void TurnLightOn()
    {
        if (_isLightTile)
        {
            _topSpriteRenderer.sprite = _lightOnSprite;
        }
    }

    public void TurnLightOff()
    {
        if (_isLightTile)
        {
            _topSpriteRenderer.sprite = _lightOffSprite;
        }
    }
    
    public float GetHeight()
    {
        return _height;
    }

    private void OnDisable()
    {
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
