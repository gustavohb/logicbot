
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Commands/MoveForward")]
public class MoveForwardCommandSO : BaseCommandSO
{
    public override void Execute()
    {
        Debug.Log("Move forward command");

        PlayerController playerController = playerControllerRuntimeSet.GetItemIndex(0);
        
        if (playerController == null)
        {
            Debug.LogError("Player controller is null!");
            return;
        }
        
        playerController.MoveForward(() =>
        {
            if (nextCommand != null)
            {
                nextCommand.Execute();
            }
        });
    }
}
