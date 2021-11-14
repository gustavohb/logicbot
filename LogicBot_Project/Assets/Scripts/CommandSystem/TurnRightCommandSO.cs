using UnityEngine;

public class TurnRightCommandSO : BaseCommandSO
{
    public override void Execute()
    {
        Debug.Log("Turn right command");
        
        if (_playerController == null)
        {
            Debug.LogError("Player controller is null!");
            return;
        }
        
        _playerController.TurnRight(() =>
        {
            if (nextCommand != null)
            {
                nextCommand.Execute();    
            }
        });
    }
}
