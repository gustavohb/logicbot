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
    
    public override string ToString()
    {
        return name;
    }
}
