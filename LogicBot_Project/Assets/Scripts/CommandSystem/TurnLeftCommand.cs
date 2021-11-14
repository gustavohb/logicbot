using UnityEngine;

public class TurnLeftCommand : BaseCommand
{
    private PlayerController _playerController;

    public TurnLeftCommand(PlayerController playerController)
    {
        _playerController = playerController;
    }
    
    public override void Execute()
    {
        Debug.Log("Turn left command");
        _playerController.TurnLeft(onFinished);
    }
}
