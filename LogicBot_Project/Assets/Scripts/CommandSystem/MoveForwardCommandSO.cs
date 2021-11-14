
using UnityEngine;

public class MoveForwardCommandSO : BaseCommandSO
{
    public override void Execute()
    {
        Debug.Log("Move forward command");
        
        if (_playerController == null)
        {
            Debug.LogError("Player controller is null!");
            return;
        }
        
        _playerController.MoveForward(() =>
        {
            if (nextCommand != null)
            {
                nextCommand.Execute();
            }
        });
    }
}
