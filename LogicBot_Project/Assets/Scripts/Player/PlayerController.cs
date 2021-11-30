using System;
using System.Collections;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public enum Dir
    {
        Right,
        Down,
        Left,
        Up,
    }

    [Header("Settings/Variables")] 
    [SerializeField] private FloatVariable _normalSpeedCommandExecutionDuration;
    [SerializeField] private FloatVariable _fastSpeedCommandExecutionDuration;
    [SerializeField] private BoolVariable _isFastModeEnabled;
    [SerializeField] private float _jumpPower = 0.7f;

    [SerializeField] private float _normalAnimatorJumpSpeed = 2.0f;
    [SerializeField] private float _fastAnimatorJumpSpeed = 4.0f;
    
    [SerializeField] private float _normalAnimatorWalkingSpeed = 2.0f;
    [SerializeField] private float _fastAnimatorWalkingSpeed = 4.0f;

    [SerializeField] private float _normalAnimatorTurnSpeed = 2.0f;
    [SerializeField] private float _fastAnimatorTurnSpeed = 4.0f;

    [SerializeField] private ColorVariable _defaultColor;
    [SerializeField] private ColorVariable _greenColor;
    [SerializeField] private ColorVariable _pinkColor;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _changeToDefaultColorEffect;
    [SerializeField] private ParticleSystem _changeToGreenColorEffect;
    [SerializeField] private ParticleSystem _changeToPinkColorEffect;
    
    [Header("References")] 
    [SerializeField] private Renderer _renderer;
    
    [Header("Events")]
    [SerializeField] private GameEvent _resetLevelGameEvent;
    [SerializeField] private ColorVariableGameEvent _setCurrentPlayerColorGameEvent;
    [SerializeField] private FloatGameEvent _setCommandDurationEvent;
    
    //[HideInInspector]
    public ColorVariable currentColor;

    private Animator _animator;
    
    private Dir _startDir;
    private Dir _currentDir;
    private float _startHeight = 0;
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private float _currentHeight;
    private Transform _transform;
    
    // Used in turn light method
    private PlacedTile _currentPlacedTile;
    private Action _currentCallback;
    
    private bool _isWalking = false;

    private ColorVariable _currentTileColor;
    private ProtagonistAudio _audio;

    private float _currentCommandDuration;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _transform = transform;
        _audio = GetComponent<ProtagonistAudio>();
    }

    private void OnEnable()
    {
        _resetLevelGameEvent.AddListener(ResetPositionRotationAndColor);
        _setCurrentPlayerColorGameEvent.AddListener(SetPlayerColor);
        _setCommandDurationEvent.AddListener(SetCommandDuration);
        SetPlayerColor(_defaultColor);

        if (_isFastModeEnabled.Value)
        {
            SetCommandDuration(_fastSpeedCommandExecutionDuration.Value);
        }
        else
        {
            SetCommandDuration(_normalSpeedCommandExecutionDuration.Value);
        }
    }

    private void SetPlayerColor(ColorVariable color)
    {
        currentColor = color;
        _renderer.material.color = currentColor.Value;
    }

    private void SetCommandDuration(float duration)
    {
        _currentCommandDuration = duration;
    }
    
    public void SetStartDir(Dir dir)
    {
        _startDir = dir;
        _currentDir = _startDir;
        _startRotation = CalculateRotationFromDir(_startDir);
        transform.rotation = _startRotation;
    }

    public void SetStartPosition(Vector3 startPosition)
    {
        _startPosition = startPosition;
    }

    public void SetStartHeight(float height)
    {
        _startHeight = height;
        _currentHeight = _startHeight;
    }

    private Quaternion CalculateRotationFromDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Up:
                return Quaternion.identity;
            case Dir.Right:
                return Quaternion.Euler(0, 90, 0);
            case Dir.Down:
                return Quaternion.Euler(0, 180, 0);
            case Dir.Left:
                return Quaternion.Euler(0, 270, 0);
        }
    }

    // Used to show current program execution
    public void DummyExecution(Action callback = null)
    {
        this.Wait(_currentCommandDuration, () => callback?.Invoke());
    }
    
    public void MoveForward(Action callback = null)
    {
        _animator.speed = _isFastModeEnabled.Value ? _fastAnimatorWalkingSpeed : _normalAnimatorWalkingSpeed;
        
        Vector3 endPosition = LevelManager.instance.GetNextPosition(transform.position, _currentDir);

        float tileHeight = endPosition.y - 1;
        CancelInvoke();
        PlayWalkingAnimation();
        Invoke(nameof(StopWalkingAnimation), _currentCommandDuration + 0.1f);
        if (_currentHeight == tileHeight)
        {
            
            transform.DOMove(endPosition, _currentCommandDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                _isWalking = false;
                _animator.SetBool("isWalking", _isWalking);
                callback?.Invoke();
            });    
        }
        else
        {
            Debug.Log("Cannot move, current height = " + _currentHeight + ", tile height = " + tileHeight);
            callback?.Invoke();
        }
    }

    private void PlayTurnLightAnimation()
    {
        Debug.LogWarning("Turn light animation is not implemented!");
        //TODO:
    }
    
    private void PlayChangeColorAnimation()
    {
        Debug.LogWarning("Change color animation is not implemented!");
        //TODO:
    }
    
    private void PlayWalkingAnimation()
    {
        _isWalking = true;
        _animator.SetBool("isWalking", _isWalking);
    }

    private void StopWalkingAnimation()
    {
        _isWalking = false;
        _animator.SetBool("isWalking", _isWalking);
    }

    public void TurnRight(Action callback = null)
    {
        _animator.speed = _isFastModeEnabled.Value ?  _fastAnimatorTurnSpeed : _normalAnimatorTurnSpeed;
        
        CancelInvoke();
        PlayWalkingAnimation();
        Invoke(nameof(StopWalkingAnimation), _currentCommandDuration + 0.1f);
        
        _currentDir = GetNextDir(_currentDir);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.DORotate(currentRotation + new Vector3(0, 90, 0), _currentCommandDuration).SetEase(Ease.Linear).OnComplete(
            () =>
            {
                callback?.Invoke();
            });
    }

    public void TurnLeft(Action callback = null)
    {
        _animator.speed = _isFastModeEnabled.Value ? _fastAnimatorTurnSpeed : _normalAnimatorTurnSpeed;
        
        CancelInvoke();
        PlayWalkingAnimation();
        Invoke(nameof(StopWalkingAnimation), _currentCommandDuration + 0.1f);
        
        _currentDir = GetPreviousDir(_currentDir);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.DORotate(currentRotation + new Vector3(0, -90, 0), _currentCommandDuration).SetEase(Ease.Linear).OnComplete(
            () =>
            {
                callback?.Invoke();
            });
    }

    public void ResetPositionRotationAndColor()
    {
        _transform.position = _startPosition;
        _transform.rotation = _startRotation;
        _currentDir = _startDir;
        _currentHeight = _startHeight;
        currentColor = _defaultColor;
        _setCurrentPlayerColorGameEvent.Raise(currentColor);
    }

    public void Jump(Action callback = null)
    {
        _animator.speed = _isFastModeEnabled.Value ?  _fastAnimatorJumpSpeed : _normalAnimatorJumpSpeed;
        _currentCallback = callback;
        _animator.SetTrigger("jump");
    }
    
    // Called via animation
    public void ExecuteJump()
    {
        Vector3 endPosition = LevelManager.instance.GetNextPosition(transform.position, _currentDir);

        float tileHeight = endPosition.y - 1;
        
        
        if ((tileHeight != _currentHeight) && (Mathf.Abs(tileHeight - _currentHeight) <= 0.5f) 
            || tileHeight < _currentHeight)
        {
            _transform.DOJump(endPosition, _jumpPower, 1, _currentCommandDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                _currentHeight = tileHeight;
                _currentCallback?.Invoke();
            });
        }
        else
        {
            _transform.DOJump(transform.position, _jumpPower, 1, _currentCommandDuration).SetEase(Ease.Linear).OnComplete(
                () =>
                {
                    _currentCallback?.Invoke();
                    //_currentCallback = null;
                });
            Debug.Log("Cannot jump, current height = " + _currentHeight + ", tile height = " + tileHeight);
        }
    }
    
    public void TurnLightOn(Action callback = null)
    {
        _currentCallback = callback;
        _currentPlacedTile = LevelManager.instance.GetPlacedTileAt(transform.position);

        switch (_currentPlacedTile.type)
        {
            default:
            case PlacedTile.PlacedTileType.Light:
                StartCoroutine(nameof(TurnLightRoutine));
                break;
            case PlacedTile.PlacedTileType.ColorSetter:
                ChangePlayerColorTo(_currentPlacedTile.GetColor());
                break;
            case PlacedTile.PlacedTileType.Teleport:
                StartCoroutine(nameof(TeleportPlayerRoutine));
                break;
        }
    }

    private void ChangePlayerColorTo(ColorVariable color)
    {
        _currentTileColor = color;
        StartCoroutine(ChangePlayerColorRoutine());
    }

    private IEnumerator TeleportPlayerRoutine()
    {
        PlacedTile teleportPlacedTileDestination = _currentPlacedTile.GetTeleportDestination();
        Vector3? teleportPosition = LevelManager.instance.GetCurrentTilePosition(teleportPlacedTileDestination.transform.position);
        yield return new WaitForSeconds(_currentCommandDuration / 2);
        if (teleportPosition.HasValue)
        {
            transform.position = teleportPosition.Value;
            _currentPlacedTile = teleportPlacedTileDestination;
            _currentHeight = _currentPlacedTile.GetHeight();
        }
        yield return new WaitForSeconds(_currentCommandDuration / 2);
        _currentCallback?.Invoke();
    }
    
    private IEnumerator ChangePlayerColorRoutine()
    {
        yield return new WaitForSeconds(_currentCommandDuration / 2);
        PlayChangeColorAnimation();

        if (_audio != null)
        {
            _audio.PlayChangeColor();
        }
        
        if (currentColor != _defaultColor)
        {
            currentColor = _defaultColor;
            _changeToDefaultColorEffect.Play();
        }
        else
        {
            currentColor = _currentTileColor;
            if (currentColor == _greenColor)
            {
                _changeToGreenColorEffect.Play();
            }
            else if (currentColor == _pinkColor)
            {
                _changeToPinkColorEffect.Play();
            }
        }
        
        yield return new WaitForSeconds(_currentCommandDuration / 2);
        
        _setCurrentPlayerColorGameEvent.Raise(currentColor);
        
        yield return new WaitForSeconds(_currentCommandDuration);
        _currentCallback?.Invoke();
    }

    private IEnumerator TurnLightRoutine()
    {
        yield return new WaitForSeconds(_currentCommandDuration / 2);
        PlayTurnLightAnimation();
        if (_currentPlacedTile != null)
        {
            _currentPlacedTile.TurnLightOn();
        }
        yield return new WaitForSeconds(_currentCommandDuration);
        _currentCallback?.Invoke();
    }
    
    private Dir GetNextDir(Dir currentDir)
    {
        switch (currentDir)
        {
            case Dir.Right:
                return Dir.Down;
            case Dir.Down:
                return Dir.Left;
            case Dir.Left:
                return Dir.Up;
            default:
            case Dir.Up:
                return Dir.Right;
        }
    }
    
    private Dir GetPreviousDir(Dir currentDir)
    {
        switch (currentDir)
        {
            case Dir.Right:
                return Dir.Up;
            case Dir.Up:
                return Dir.Left;
            case Dir.Left:
                return Dir.Down;
            default:
            case Dir.Down:
                return Dir.Right;
        }
    }

    private void OnDisable()
    {
        _resetLevelGameEvent.RemoveListener(ResetPositionRotationAndColor);
        _setCurrentPlayerColorGameEvent.RemoveListener(SetPlayerColor);
        _setCommandDurationEvent.RemoveListener(SetCommandDuration);
    }
}
