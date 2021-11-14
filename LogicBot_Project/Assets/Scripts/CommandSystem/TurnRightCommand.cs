using UnityEngine;

public class TurnRightCommand : BaseCommand
{
    private PlayerController _playerController;

    public TurnRightCommand(PlayerController playerController)
    {
        _playerController = playerController;
    }
    
    public override void Execute()
    {
        Debug.Log("Turn right command");
        _playerController.TurnRight(onFinished);
    }
}
