using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class CommandProcessor : Singleton<CommandProcessor>
{
    private List<BaseCommand> _commandList = new List<BaseCommand>();
    
    [SerializeField] private GameEvent _resetPlayerPositionGameEvent;
    
    public void Add(BaseCommand baseCommand)
    {
        _commandList.Add(baseCommand);
        
        if (_commandList.Count - 2 >= 0)
        {
            BaseCommand previousCommand = _commandList[_commandList.Count - 2];
            previousCommand.onFinished += baseCommand.Execute;    
        }
    }

    public void ExecuteCommands()
    {
        if (_commandList == null || _commandList.Count == 0)
        {
            return;
        }
        
        _resetPlayerPositionGameEvent.Raise();
        _commandList[0].Execute();
    }

    private void ConfigureOnFinishedCallbacksToCallNextCommandInList()
    {
        BaseCommand previousBaseCommand = _commandList[0];
        BaseCommand currentBaseCommand;
        for (int i = 1; i < _commandList.Count; i++)
        {
            currentBaseCommand = _commandList[i];
            previousBaseCommand.onFinished += currentBaseCommand.Execute;
            previousBaseCommand = currentBaseCommand;
        }
    }

    private void ClearOnFinishedCallbacksInCommandList()
    {
        for (int i = 0; i < _commandList.Count; i++)
        {
            _commandList[i].onFinished = null;
        }
    }
    
    public void ClearAllCommands()
    {
        _commandList.Clear();
    }
    
}
