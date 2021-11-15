using UnityEngine;

[CreateAssetMenu(menuName = "Game/Commands/Jump")]
public class JumpCommandSO : BaseCommandSO
{
    public override void Execute()
    {
        Debug.Log("Jump command");

        PlayerController playerController = playerControllerRuntimeSet.GetItemIndex(0);
        
        if (playerController == null)
        {
            Debug.LogError("Player controller is null!");
            return;
        }
        
        playerController.Jump(() =>
        {
            if (nextCommand != null)
            {
                nextCommand.Execute();
            }
        });

    }
}
