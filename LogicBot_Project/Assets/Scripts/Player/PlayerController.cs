using System;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;

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

    [SerializeField] private float _jumpPower = 0.7f;
    
    [SerializeField] private GameEvent _resetLevelGameEvent;

    [SerializeField] private float _startHeight = 0;
    
    private Dir _currentDir = Dir.Up;

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Dir _startDir;
    private float _currentHeight;

    private void OnEnable()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        _startDir = _currentDir;
        _currentHeight = _startHeight;
        _resetLevelGameEvent.AddListener(ResetPosition);
    }
    

    public void MoveForward(Action callback = null)
    {
        Debug.Log("Move forward");
        Vector3 endPosition = LevelManager.instance.GetNextPosition(transform.position, _currentDir);

        float tileHeight = endPosition.y - 1;
        
        if (_currentHeight == tileHeight)
        {
            transform.DOMove(endPosition, _moveDuration.Value).SetEase(Ease.Linear).OnComplete(() =>
            {
                callback?.Invoke();
            });    
        }
        else
        {
            Debug.Log("Cannot move, current height = " + _currentHeight + ", tile height = " + tileHeight);
        }
    }

    public void TurnRight(Action callback = null)
    {
        Debug.Log("Turn right");
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
        _currentDir = GetPreviousDir(_currentDir);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.DORotate(currentRotation + new Vector3(0, -90, 0), _turnDuration.Value).SetEase(Ease.Linear).OnComplete(
            () =>
            {
                callback?.Invoke();
            });
    }

    public void ResetPosition()
    {
        transform.position = _startPosition;
        transform.rotation = _startRotation;
        _currentDir = _startDir;
        _currentHeight = _startHeight;
    }
    
    public void Jump(Action callback = null)
    {
        Debug.Log("Jump");
        Vector3 endPosition = LevelManager.instance.GetNextPosition(transform.position, _currentDir);

        float tileHeight = endPosition.y - 1;
        
        if (tileHeight != _currentHeight && Mathf.Abs(tileHeight - _currentHeight) <= 0.5f)
        {
            transform.DOJump(endPosition, _jumpPower, 1, _moveDuration.Value).SetEase(Ease.Linear).OnComplete(() =>
            {
                _currentHeight = tileHeight;
                callback?.Invoke();
            });    
        }
        else
        {
            transform.DOJump(transform.position, _jumpPower, 1, _moveDuration.Value).SetEase(Ease.Linear).OnComplete(() =>
            {
                callback?.Invoke();
            });  
            Debug.Log("Cannot jump, current height = " + _currentHeight + ", tile height = " + tileHeight);
        }

    }

    public void TurnLightOn(Action callback = null)
    {
        Debug.Log("Turn light on");

        PlacedTile placedTile = LevelManager.instance.GetPlacedTileAt(transform.position);
        if (placedTile != null)
        {
            placedTile.TurnLightOn();
        }
        
        callback?.Invoke();
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
        _resetLevelGameEvent.RemoveListener(ResetPosition);
    }
}
