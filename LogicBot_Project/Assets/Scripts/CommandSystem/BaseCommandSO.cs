using System;
using UnityEngine;

[Serializable]
public abstract class BaseCommandSO : ScriptableObject
{
    public BaseCommandSO nextCommand;

    public PlayerControllerRuntimeSet playerControllerRuntimeSet;

    public abstract void Execute();

}
