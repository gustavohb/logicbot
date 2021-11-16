using ScriptableObjectArchitecture;
using UnityEngine;

public class CommandProcessor : Singleton<CommandProcessor>
{
    [SerializeField] private ProgramListCommandSO _mainProgramListCommand;
    [SerializeField] private ProgramListCommandSO _proc1ListCommand;
    [SerializeField] private ProgramListCommandSO _proc2ListCommand;
    
    [SerializeField] private GameEvent _resetLevelGameEvent;

    private void OnEnable()
    {
        ClearAllCommands();
    }

    public void ExecuteCommands()
    {
        _resetLevelGameEvent.Raise();
        
        if (_mainProgramListCommand == null || _mainProgramListCommand.commandList == null || _mainProgramListCommand.commandList.Count == 0)
        {
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
