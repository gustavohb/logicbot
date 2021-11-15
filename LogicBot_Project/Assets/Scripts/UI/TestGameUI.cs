using UnityEngine;
using UnityEngine.UI;

public class TestGameUI : MonoBehaviour
{
    [SerializeField] private Button _moveForwardButton;
    [SerializeField] private Button _turnLeftButton;
    [SerializeField] private Button _turnRightButton;
    [SerializeField] private Button _jumpButton;
    [SerializeField] private Button _turnLightOnButton;
    [SerializeField] private Button _playButton;

    [SerializeField] private PlayerControllerRuntimeSet _playerControllerRuntimeSet;
    
    private void Start()
    {
        // _moveForwardButton.onClick.AddListener(AddMoveForwardCommand);
        // _turnLeftButton.onClick.AddListener(AddTurnLeftCommand);
        // _turnRightButton.onClick.AddListener(AddTurnRightCommand);
        // _jumpButton.onClick.AddListener(AddJumpCommand);
        // _turnLightOnButton.onClick.AddListener(AddTurnLightOnCommand);
        _playButton.onClick.AddListener(ExecuteCommands);
    }


    public void AddJumpCommand()
    {
        JumpCommandSO newJumpCommand = ScriptableObject.CreateInstance<JumpCommandSO>();
        CommandProcessor.instance.Add(newJumpCommand);
    }
    
    public void AddMoveForwardCommand()
    {
        MoveForwardCommandSO newMoveForwardCommand = ScriptableObject.CreateInstance<MoveForwardCommandSO>();
        CommandProcessor.instance.Add(newMoveForwardCommand);
    }

    public void AddTurnLeftCommand()
    {
        TurnLeftCommandSO newTurnLeftCommand = ScriptableObject.CreateInstance<TurnLeftCommandSO>();
        CommandProcessor.instance.Add(newTurnLeftCommand);
    }
    
    public void AddTurnRightCommand()
    {
        TurnRightCommandSO newTurnRightCommand = ScriptableObject.CreateInstance<TurnRightCommandSO>();
        CommandProcessor.instance.Add(newTurnRightCommand);
    }

    public void AddTurnLightOnCommand()
    {
        TurnLightOnCommandSO newTurnLightOnCommand = ScriptableObject.CreateInstance<TurnLightOnCommandSO>();
        CommandProcessor.instance.Add(newTurnLightOnCommand);
    }

    public void ExecuteCommands()
    {
        CommandProcessor.instance.ExecuteCommands();
    }
}
