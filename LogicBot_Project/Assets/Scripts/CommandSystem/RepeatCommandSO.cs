using UnityEngine;

public class RepeatCommandSO : BaseCommandSO
{
    public override void Execute()
    {
        Debug.Log("Repeat command");
        if (nextCommand != null)
        {
            nextCommand.Execute();
        }
    }
}
