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
    
    public abstract void Execute(Action callback);
}
