
using UnityEngine;

public class MoveForwardCommand : BaseCommand
{
    private PlayerController _playerController;

    public MoveForwardCommand(PlayerController playerController)
    {
        _playerController = playerController;
    }
    
    public override void Execute()
    {
        Debug.Log("Move forward command");
        _playerController.MoveForward(onFinished);
    }
}
