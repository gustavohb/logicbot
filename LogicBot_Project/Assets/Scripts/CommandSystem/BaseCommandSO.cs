using System;
using UnityEngine;

[Serializable]
public abstract class BaseCommandSO : ScriptableObject
{
    public PlayerControllerRuntimeSet playerControllerRuntimeSet;

    public bool isCommandList = false;
    public ProgramListCommandSO parentListCommand;
    
    public abstract void Execute(Action callback);
}
