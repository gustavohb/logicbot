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

    [SerializeField] private FloatVariable _moveDuration;
    [SerializeField] private FloatVariable _turnDuration;
    [SerializeField] private FloatVariable _turnLightDuration;
    
    [SerializeField] private float _jumpPower = 0.7f;
    
    [SerializeField] private GameEvent _resetLevelGameEvent;

    [SerializeField] private float _startHeight = 0;

    [SerializeField] private float _animatorWalkingSpeed = 1.5f;
    [SerializeField] private float _animatorTurnSpeed = 1.0f;

    [SerializeField] private Dir _startDir;

    [SerializeField] private float _jumpDelay = 0.1f;
    
    private Animator _animator;
    [SerializeField] private Dir _currentDir;
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private float _currentHeight;

    // Used in turn light method
    private PlacedTile _currentPlacedTile;
    private Action _currentCallback;
    
    private bool _isWalking = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _resetLevelGameEvent.AddListener(ResetPositionAndRotation);
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
    
    private Dir CalculateStartDir()
    {
        if (_startRotation == Quaternion.Euler(0, 0, 0))
        {
            _startDir = Dir.Up;
        }
        else if (_startRotation == Quaternion.Euler(0, 90, 0))
        {
            _startDir = Dir.Right;
        }
        else if (_startRotation == Quaternion.Euler(0, 180, 0))
        {
            _startDir = Dir.Down;
        }
        else if (_startRotation == Quaternion.Euler(0, 270, 0))
        {
            _startDir = Dir.Left;
        }

        return _startDir;
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

    public void ResetPositionAndRotation()
    {
        transform.position = _startPosition;
        transform.rotation = _startRotation;
        _currentDir = _startDir;
        _currentHeight = _startHeight;
    }
    
    
    public void Jump(Action callback = null)
    {
        Debug.Log("Jump");
        _currentCallback = callback;
        _animator.SetTrigger("jump");
    }

    public void ExecuteJump()
    {
        Vector3 endPosition = LevelManager.instance.GetNextPosition(transform.position, _currentDir);

        float tileHeight = endPosition.y - 1;
        
        if (tileHeight != _currentHeight && Mathf.Abs(tileHeight - _currentHeight) <= 0.5f)
        {
            transform.DOJump(endPosition, _jumpPower, 1, _moveDuration.Value).SetEase(Ease.Linear).OnComplete(() =>
            {
                _currentHeight = tileHeight;
                _currentCallback?.Invoke();
                //_currentCallback = null;
            });
        }
        else
        {
            transform.DOJump(transform.position, _jumpPower, 1, _moveDuration.Value).SetEase(Ease.Linear).OnComplete(
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
        StartCoroutine(nameof(TurnLightRoutine));
        Debug.Log("Turn light ended");
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
        Debug.Log("Current callback invoked");
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
        _resetLevelGameEvent.RemoveListener(ResetPositionAndRotation);
    }
}
