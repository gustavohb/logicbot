using UnityEngine;

[CreateAssetMenu(menuName = "Game/Commands/TurnRight")]
public class TurnRightCommandSO : BaseCommandSO
{
    public override void Execute()
    {
        Debug.Log("Turn right command");

        PlayerController playerController = playerControllerRuntimeSet.GetItemIndex(0);
        
        if (playerController == null)
        {
            Debug.LogError("Player controller is null!");
            return;
        }
        
        playerController.TurnRight(() =>
        {
            if (nextCommand != null)
            {
                nextCommand.Execute();    
            }
        });
    }
}
