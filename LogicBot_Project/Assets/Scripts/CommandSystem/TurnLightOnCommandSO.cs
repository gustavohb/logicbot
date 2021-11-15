using UnityEngine;

[CreateAssetMenu(menuName = "Game/Commands/TurnLightOn")]
public class TurnLightOnCommandSO : BaseCommandSO
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
        
        playerController.TurnLightOn(() =>
        {
            if (nextCommand != null)
            {
                nextCommand.Execute();
            }
        });
    }
}