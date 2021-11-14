using UnityEngine;

public class JumpCommandSO : BaseCommandSO
{
    public override void Execute()
    {
        Debug.Log("Jump command");
        if (_playerController == null)
        {
            Debug.LogError("Player controller is null!");
            return;
        }
        
        _playerController.Jump(() =>
        {
            if (nextCommand != null)
            {
                nextCommand.Execute();
            }
        });

    }
}
