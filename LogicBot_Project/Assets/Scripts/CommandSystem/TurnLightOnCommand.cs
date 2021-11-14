using UnityEngine;

public class TurnLightOnCommand : BaseCommand
{
    private PlayerController _playerController;

    public TurnLightOnCommand(PlayerController playerController)
    {
        _playerController = playerController;
    }
    
    public override void Execute()
    {
        Debug.Log("Move forward command");
        _playerController.TurnLightOn(onFinished);
    }
}