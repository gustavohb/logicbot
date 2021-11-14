using UnityEngine;

public class JumpCommand : BaseCommand
{
    private PlayerController _playerController;

    public JumpCommand(PlayerController playerController)
    {
        _playerController = playerController;
    }
    
    public override void Execute()
    {
        Debug.Log("Jump command");
        _playerController.Jump(onFinished);
    }
}
