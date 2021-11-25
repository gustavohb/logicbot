using System;
using ScriptableObjectArchitecture;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Commands/ProgramListCommand")]
public class ProgramListCommandSO : BaseCommandSO
{
    [Header("Program List Settings")]
    public bool isMainProgram = false;
    
    [Header("Program List References")]
    public CommandListSO commandList;

    [Header("Program List Events")]
    public GameEvent onFinishedExecutionGameEvent;
    
    private int _currentCommandIndex = 0;
    
    public void Add(BaseCommandSO baseCommand)
    {
        commandList.Add(baseCommand);
    }

    public void ClearCommands()
    {
        commandList.Clear();
        parentListCommand = null;
    }
    
    public override void Execute(Action callback)
    {
        Debug.Log("Execute " + name);
        if (stopped.Value)
        {
            return;
        }
        
        if (currentPlayerColor != commandColor && commandColor != defaultPlayerColor)
        {
            callback?.Invoke();
            return;
        }

        if (commandList != null && commandList.Count > 0)
        {
            _currentCommandIndex = 0;
            BaseCommandSO firstCommand = commandList[_currentCommandIndex];
            firstCommand.parentListCommand = this;
            firstCommand.Execute(ContinueExecution);
        }
        else if (isMainProgram)
        {
            stopped.Value = true;
            onFinishedExecutionGameEvent.Raise();
        }
    }

    public void ContinueExecution()
    {
        if (stopped.Value)
        {
            return;
        }
        
        _currentCommandIndex++;
        if (_currentCommandIndex < commandList.Count)
        {
            BaseCommandSO nextCommand = commandList[_currentCommandIndex];
            
            if (nextCommand.isBreakCommand && nextCommand.parentListCommand == null) // Why only works when add nextCommand.parentListCommand == null 
            {
                nextCommand.parentListCommand = parentListCommand;
            }
            
            if (nextCommand.isCommandList)
            {
                nextCommand.parentListCommand = this;
            }
            nextCommand.Execute(ContinueExecution); 
        }
        else if (parentListCommand != null)
        {
            _currentCommandIndex = 0;
            parentListCommand.ContinueExecution();
        }
        else if (isMainProgram)
        {
            onFinishedExecutionGameEvent.Raise();
        }
    }
}
