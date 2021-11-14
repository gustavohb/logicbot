using System;
using UnityEngine;

[Serializable]
public abstract class BaseCommandSO : ScriptableObject
{
    public BaseCommandSO nextCommand;
    
    protected PlayerController _playerController;

    public abstract void Execute();

    public virtual void SetPlayerController(PlayerController playerController)
    {
        _playerController = playerController;
    }
    
    
}
