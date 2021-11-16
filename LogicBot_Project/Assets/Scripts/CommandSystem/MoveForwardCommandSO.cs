
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Commands/MoveForward")]
public class MoveForwardCommandSO : BaseCommandSO
{
    public override void Execute(Action callback)
    {
        if (stopped.Value)
        {
            return;
        }
        
        Debug.Log("Move forward command");

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

        for (int i = 0; i < playerControllers.Length; i++)
        {
            int n = i;
            playerControllers[i].MoveForward(() =>
            {
                if (n == 0)
                {
                    callback?.Invoke();  // Just invoke callback once
                }
            });
        }
    }
}
