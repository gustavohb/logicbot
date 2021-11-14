using UnityEngine;
using UnityEngine.UI;

public class TestGameUI : MonoBehaviour
{
    [SerializeField] private ProgramListCommandSO _mainProgramListCommand;
    
    [SerializeField] private Button _moveForwardButton;
    [SerializeField] private Button _turnLeftButton;
    [SerializeField] private Button _turnRightButton;
    [SerializeField] private Button _jumpButton;
    [SerializeField] private Button _turnLightOnButton;
    [SerializeField] private Button _repeatButton;
    [SerializeField] private Button _playButton;

    [SerializeField] private PlayerControllerRuntimeSet _playerControllerRuntimeSet;
    
    private void Start()
    {
        _moveForwardButton.onClick.AddListener(AddMoveForwardCommand);
        _turnLeftButton.onClick.AddListener(AddTurnLeftCommand);
        _turnRightButton.onClick.AddListener(AddTurnRightCommand);
        _jumpButton.onClick.AddListener(AddJumpCommand);
        _turnLightOnButton.onClick.AddListener(AddTurnLightOnCommand);
        _repeatButton.onClick.AddListener(AddRepeatCommand);
        _playButton.onClick.AddListener(ExecuteCommands);
    }

    private void AddRepeatCommand()
    {
        CommandProcessor.instance.Add(_mainProgramListCommand);
    }

    public void AddJumpCommand()
    {
        JumpCommandSO newJumpCommand = ScriptableObject.CreateInstance<JumpCommandSO>();
        newJumpCommand.SetPlayerController(_playerControllerRuntimeSet.GetItemIndex(0));
        CommandProcessor.instance.Add(newJumpCommand);
    }
    
    public void AddMoveForwardCommand()
    {
        MoveForwardCommandSO newMoveForwardCommand = ScriptableObject.CreateInstance<MoveForwardCommandSO>();
        newMoveForwardCommand.SetPlayerController(_playerControllerRuntimeSet.GetItemIndex(0));
        CommandProcessor.instance.Add(newMoveForwardCommand);
    }

    public void AddTurnLeftCommand()
    {
        TurnLeftCommandSO newTurnLeftCommand = ScriptableObject.CreateInstance<TurnLeftCommandSO>();
        newTurnLeftCommand.SetPlayerController(_playerControllerRuntimeSet.GetItemIndex(0));
        CommandProcessor.instance.Add(newTurnLeftCommand);
    }
    
    public void AddTurnRightCommand()
    {
        TurnRightCommandSO newTurnRightCommand = ScriptableObject.CreateInstance<TurnRightCommandSO>();
        newTurnRightCommand.SetPlayerController(_playerControllerRuntimeSet.GetItemIndex(0));
        CommandProcessor.instance.Add(newTurnRightCommand);
    }

    public void AddTurnLightOnCommand()
    {
        TurnLightOnCommandSO newTurnLightOnCommand = ScriptableObject.CreateInstance<TurnLightOnCommandSO>();
        newTurnLightOnCommand.SetPlayerController(_playerControllerRuntimeSet.GetItemIndex(0));
        CommandProcessor.instance.Add(newTurnLightOnCommand);
    }

    public void ExecuteCommands()
    {
        CommandProcessor.instance.ExecuteCommands();
    }
}
