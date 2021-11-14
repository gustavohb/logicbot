using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Commands/ProgramListCommand")]
public class ProgramListCommandSO : BaseCommandSO
{
    public List<BaseCommandSO> commandList = new List<BaseCommandSO>();

    public void Add(BaseCommandSO baseCommand)
    {
        BaseCommandSO previousCommand = null;
        if (commandList.Count > 0)
        {
            previousCommand = commandList[commandList.Count - 1];
            previousCommand.nextCommand = baseCommand;
        }
        commandList.Add(baseCommand);
    }

    public void ClearCommands()
    {
        commandList.Clear();
    }
    
    
    public override void Execute()
    {
        if (commandList != null && commandList.Count > 0)
        {
            commandList[0].Execute();
        }
    }
}
