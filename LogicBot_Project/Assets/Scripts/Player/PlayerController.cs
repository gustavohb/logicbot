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
    [SerializeField] private FloatVariable _moveDuration;
    [SerializeField] private FloatVariable _turnDuration;
    [SerializeField] private FloatVariable _turnLightDuration;
    [SerializeField] private FloatVariable _changeColorDuration;
    [SerializeField] private float _jumpPower = 0.7f;
    [SerializeField] private float _animatorWalkingSpeed = 1.5f;
    [SerializeField] private float _animatorTurnSpeed = 1.0f;
    [SerializeField] private ColorVariable _defaultColor;

    [Header("References")] 
    [SerializeField] private Renderer _renderer;
    
    [Header("Events")]
    [SerializeField] private GameEvent _resetLevelGameEvent;
    [SerializeField] private ColorVariableGameEvent _setCurrentPlayerColorGameEvent;
    
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

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _transform = transform;
    }

    private void OnEnable()
    {
        _resetLevelGameEvent.AddListener(ResetPositionRotationAndColor);
        _setCurrentPlayerColorGameEvent.AddListener(SetPlayerColor);
        SetPlayerColor(_defaultColor);
    }

    private void SetPlayerColor(ColorVariable color)
    {
        Debug.Log("Set player material");
        currentColor = color;
        _renderer.material.color = currentColor.Value;
        //TODO: Change player color
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
    
    public void MoveForward(Action callback = null)
    {
        _animator.speed = _animatorWalkingSpeed;
        
        Debug.Log("Move forward");
        Vector3 endPosition = LevelManager.instance.GetNextPosition(transform.position, _currentDir);

        float tileHeight = endPosition.y - 1;
        CancelInvoke();
        PlayWalkingAnimation();
        Invoke(nameof(StopWalkingAnimation), _moveDuration.Value + 0.1f);
        if (_currentHeight == tileHeight)
        {
            
            transform.DOMove(endPosition, _moveDuration.Value).SetEase(Ease.Linear).OnComplete(() =>
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
        Debug.Log("Play turn light animation");
        //TODO:
    }
    
    private void PlayChangeColorAnimation()
    {
        Debug.Log("Play change color animation");
        //TODO:
    }
    
    private void PlayWalkingAnimation()
    {
        Debug.Log("Play walking animation");
        _isWalking = true;
        _animator.SetBool("isWalking", _isWalking);
    }

    private void StopWalkingAnimation()
    {
        Debug.Log("Stop walking animation");
        _isWalking = false;
        _animator.SetBool("isWalking", _isWalking);
    }
    

    public void TurnRight(Action callback = null)
    {
        Debug.Log("Turn right");

        _animator.speed = _animatorTurnSpeed;
        
        CancelInvoke();
        PlayWalkingAnimation();
        Invoke(nameof(StopWalkingAnimation), _turnDuration.Value + 0.1f);
        
        _currentDir = GetNextDir(_currentDir);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.DORotate(currentRotation + new Vector3(0, 90, 0), _turnDuration.Value).SetEase(Ease.Linear).OnComplete(
            () =>
            {
                callback?.Invoke();
            });
    }

    public void TurnLeft(Action callback = null)
    {
        Debug.Log("Turn left");
        
        _animator.speed = _animatorTurnSpeed;
        
        CancelInvoke();
        PlayWalkingAnimation();
        Invoke(nameof(StopWalkingAnimation), _turnDuration.Value + 0.1f);
        
        _currentDir = GetPreviousDir(_currentDir);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.DORotate(currentRotation + new Vector3(0, -90, 0), _turnDuration.Value).SetEase(Ease.Linear).OnComplete(
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
        Debug.Log("Jump");
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
            _transform.DOJump(endPosition, _jumpPower, 1, _moveDuration.Value).SetEase(Ease.Linear).OnComplete(() =>
            {
                _currentHeight = tileHeight;
                _currentCallback?.Invoke();
            });
        }
        else
        {
            _transform.DOJump(transform.position, _jumpPower, 1, _moveDuration.Value).SetEase(Ease.Linear).OnComplete(
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
        Debug.Log("Turn light on");
        _currentCallback = callback;
        _currentPlacedTile = LevelManager.instance.GetPlacedTileAt(transform.position);
        
        if (_currentPlacedTile.isColorSetter)
        {
            ChangePlayerColorTo(_currentPlacedTile.GetColor());
        }
        else
        {
            StartCoroutine(nameof(TurnLightRoutine));   
        }
    }

    private void ChangePlayerColorTo(ColorVariable color)
    {
        _currentTileColor = color;
        StartCoroutine(ChangePlayerColorRoutine());
    }

    private IEnumerator ChangePlayerColorRoutine()
    {
        Debug.Log("Change player color");
        yield return new WaitForSeconds(.15f);
        PlayChangeColorAnimation();

        if (currentColor != _defaultColor)
        {
            currentColor = _defaultColor;
        }
        else
        {
            currentColor = _currentTileColor;
        }
        
        _setCurrentPlayerColorGameEvent.Raise(currentColor);
        
        yield return new WaitForSeconds(_changeColorDuration.Value);
        _currentCallback?.Invoke();
    }

    private IEnumerator TurnLightRoutine()
    {
        Debug.Log("Turn light routine");
        yield return new WaitForSeconds(.15f);
        PlayTurnLightAnimation();
        if (_currentPlacedTile != null)
        {
            _currentPlacedTile.TurnLightOn();
        }
        yield return new WaitForSeconds(_turnLightDuration.Value);
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
    }
}
