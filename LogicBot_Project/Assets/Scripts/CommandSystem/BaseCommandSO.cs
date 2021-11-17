using System;
using ScriptableObjectArchitecture;
using UnityEngine;

[Serializable]
public abstract class BaseCommandSO : ScriptableObject
{
    public PlayerControllerRuntimeSet playerControllerRuntimeSet;

    public bool isCommandList = false;
    public ProgramListCommandSO parentListCommand;

    public BoolVariable stopped;

    public bool isExecuting = false;

    protected CommandUI _commandUI;

    protected Action _callback;
    
    public virtual void SetCommandUI(CommandUI commandUI)
    {
        _commandUI = commandUI;
    }
    
    public abstract void Execute(Action callback);
}
