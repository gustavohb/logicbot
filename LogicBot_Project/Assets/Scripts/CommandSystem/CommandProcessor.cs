using ScriptableObjectArchitecture;
using UnityEngine;

public class CommandProcessor : Singleton<CommandProcessor>
{
    [SerializeField] private ProgramListCommandSO _mainProgramListCommand;
    [SerializeField] private ProgramListCommandSO _proc1ListCommand;
    [SerializeField] private ProgramListCommandSO _proc2ListCommand;

    [SerializeField] private BoolVariable _stopped;
    
    [SerializeField] private GameEvent _resetLevelGameEvent;
    [SerializeField] private GameEvent onFinishedExecutionGameEvent;
    private void OnEnable()
    {
        ClearAllCommands();
    }

    public void ExecuteCommands()
    {
        _resetLevelGameEvent.Raise();

        _stopped.Value = false;
        
        if (_mainProgramListCommand == null || _mainProgramListCommand.commandList == null || _mainProgramListCommand.commandList.Count == 0)
        {
            onFinishedExecutionGameEvent.Raise();
            return;
            
        }

        _mainProgramListCommand.Execute(null);
    }

    public void ClearAllCommands()
    {
        _mainProgramListCommand.ClearCommands();
        _proc1ListCommand.ClearCommands();
        _proc2ListCommand.ClearCommands();
    }
    
}
