using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Commands/TurnLeft")]
public class TurnLeftCommandSO : BaseCommandSO
{
    public override void Execute(Action callback)
    {
        if (stopped.Value)
        {
            return;
        }
        
        Debug.Log("Turn left command");

        if (playerControllerRuntimeSet == null)
        {
            Debug.LogError("Player runtime set is not set!");
            return;
        }
        
        PlayerController[] playerControllers = playerControllerRuntimeSet.GetAll();
        
        if (playerControllers == null)
        {
            Debug.LogError("Player controllers are null!");
            return;
        }
        
        if (_commandUI != null)
        {
            _commandUI.SetAsExecuting();
        }

        for (int i = 0; i < playerControllers.Length; i++)
        {
            int n = i;
            playerControllers[i].TurnLeft(() =>
            {
                if (n == 0)
                {
                    callback?.Invoke();  // Just invoke callback once
                    if (_commandUI != null)
                    {
                        _commandUI.SetAsNotExecuting();
                    }
                }
            });
        }
    }
}
