using UnityEngine;

[CreateAssetMenu(menuName = "Game/Commands/TurnLeft")]
public class TurnLeftCommandSO : BaseCommandSO
{
    public override void Execute()
    {
        Debug.Log("Turn left command");

        PlayerController playerController = playerControllerRuntimeSet.GetItemIndex(0);
        
        if (playerController == null)
        {
            Debug.LogError("Player controller is null!");
            return;
        }
        
        playerController.TurnLeft(() =>
        {
            if (nextCommand != null)
            {
                nextCommand.Execute();
            }
        });
    }
}
