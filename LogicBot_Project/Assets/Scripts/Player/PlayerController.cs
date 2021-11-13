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
    
    [SerializeField] private GameEvent _moveForwardGameEvent;
    [SerializeField] private GameEvent _turnLeftGameEvent;
    [SerializeField] private GameEvent _turnRightGameEvent;
    
    private Dir _currentDir = Dir.Up;

    private void OnEnable()
    {
        _moveForwardGameEvent.AddListener(MoveForward);
        _turnLeftGameEvent.AddListener(TurnLeft);
        _turnRightGameEvent.AddListener(TurnRight);
    }

    public void MoveForward()
    {
        Debug.Log("Move forward");
        Vector3 endPosition = LevelManager.instance.GetNextPosition(transform.position, _currentDir);
        transform.DOMove(endPosition, _moveDuration.Value).SetEase(Ease.Linear);
    }

    public void TurnRight()
    {
        Debug.Log("Turn right");
        _currentDir = GetNextDir(_currentDir);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.DORotate(currentRotation + new Vector3(0, 90, 0), _moveDuration.Value).SetEase(Ease.Linear);
    }

    public void TurnLeft()
    {
        Debug.Log("Turn left");
        _currentDir = GetPreviousDir(_currentDir);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.DORotate(currentRotation + new Vector3(0, -90, 0), _moveDuration.Value).SetEase(Ease.Linear);
    }

    public void Jump()
    {
        Debug.Log("Jump");
    }

    public void TurnLightOn()
    {
        Debug.Log("Turn light on");
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
        _moveForwardGameEvent.RemoveListener(MoveForward);
        _turnLeftGameEvent.RemoveListener(TurnLeft);
        _turnRightGameEvent.RemoveListener(TurnRight);
    }
}
