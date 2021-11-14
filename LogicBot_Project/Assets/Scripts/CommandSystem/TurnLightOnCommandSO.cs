using UnityEngine;

public class TurnLightOnCommandSO : BaseCommandSO
{
    public override void Execute()
    {
        Debug.Log("Move forward command");
        
        if (_playerController == null)
        {
            Debug.LogError("Player controller is null!");
            return;
        }
        
        _playerController.TurnLightOn(() =>
        {
            if (nextCommand != null)
            {
                nextCommand.Execute();
            }
        });
    }
}