using UnityEngine;
using UnityEngine.UI;

public class TestGameUI : MonoBehaviour
{
    [SerializeField] private Button _moveForwardButton;
    [SerializeField] private Button _turnLeftButton;
    [SerializeField] private Button _turnRightButton;
    [SerializeField] private Button _jumpButton;
    [SerializeField] private Button _playButton;

    [SerializeField] private PlayerControllerRuntimeSet _playerControllerRuntimeSet;
    
    private void Start()
    {
        _moveForwardButton.onClick.AddListener(AddMoveForwardCommand);
        _turnLeftButton.onClick.AddListener(AddTurnLeftCommand);
        _turnRightButton.onClick.AddListener(AddTurnRightCommand);
        _jumpButton.onClick.AddListener(AddJumpCommand);
        _playButton.onClick.AddListener(ExecuteCommands);
    }

    public void AddJumpCommand()
    {
        JumpCommand newJumpCommand = new JumpCommand(_playerControllerRuntimeSet.GetItemIndex(0));
        CommandProcessor.instance.Add(newJumpCommand);
    }
    
    public void AddMoveForwardCommand()
    {
        MoveForwardCommand newMoveForwardCommand = new MoveForwardCommand(_playerControllerRuntimeSet.GetItemIndex(0));
        CommandProcessor.instance.Add(newMoveForwardCommand);
    }

    public void AddTurnLeftCommand()
    {
        TurnLeftCommand newTurnLeftCommand = new TurnLeftCommand(_playerControllerRuntimeSet.GetItemIndex(0));
        CommandProcessor.instance.Add(newTurnLeftCommand);
    }
    
    public void AddTurnRightCommand()
    {
        TurnRightCommand newTurnRightCommand = new TurnRightCommand(_playerControllerRuntimeSet.GetItemIndex(0));
        CommandProcessor.instance.Add(newTurnRightCommand);
    }

    public void ExecuteCommands()
    {
        CommandProcessor.instance.ExecuteCommands();
    }
}
