using ScriptableObjectArchitecture;
using UnityEngine;

public class CommandProcessor : Singleton<CommandProcessor>
{
    [SerializeField] private ProgramListCommandSO _mainProgramListCommand;
    
    [SerializeField] private GameEvent _resetLevelGameEvent;

    private void OnEnable()
    {
        ClearAllCommands();
    }

    public void Add(BaseCommandSO baseCommand)
    {
        _mainProgramListCommand.Add(baseCommand);
    }

    public void ExecuteCommands()
    {
        if (_mainProgramListCommand == null || _mainProgramListCommand.commandList == null || _mainProgramListCommand.commandList.Count == 0)
        {
            return;
        }

        _resetLevelGameEvent.Raise();
        _mainProgramListCommand.Execute();
    }

    public void ClearAllCommands()
    {
        _mainProgramListCommand.ClearCommands();
    }
    
}
