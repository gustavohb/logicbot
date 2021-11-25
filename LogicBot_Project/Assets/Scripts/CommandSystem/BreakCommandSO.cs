
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Commands/Break")]
public class BreakCommandSO : BaseCommandSO
{
    public override void Execute(Action callback)
    {
        if (stopped.Value)
        {
            return;
        }
        
        if (currentPlayerColor != commandColor && commandColor != defaultPlayerColor)
        {
            callback?.Invoke();
            return;
        }
        
        Debug.Log("Break command");

        if (_commandUI != null)
        {
            _commandUI.SetAsExecuting();
        }

        if (parentListCommand != null)
        {
            Debug.Log("Continue execution " + parentListCommand.name);
            parentListCommand.ContinueExecution();    
        }
    }
}
