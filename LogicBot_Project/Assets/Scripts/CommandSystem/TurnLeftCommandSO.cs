using UnityEngine;

public class TurnLeftCommandSO : BaseCommandSO
{
    public override void Execute()
    {
        Debug.Log("Turn left command");
        
        if (_playerController == null)
        {
            Debug.LogError("Player controller is null!");
            return;
        }
        
        _playerController.TurnLeft(() =>
        {
            if (nextCommand != null)
            {
                nextCommand.Execute();
            }
        });
    }
}
