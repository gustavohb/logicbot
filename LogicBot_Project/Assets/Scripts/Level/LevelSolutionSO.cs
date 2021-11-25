using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Level/LevelSolution")]
public class LevelSolutionSO : ScriptableObject
{
    public int possibleCommandQtyInMainCommands;
    public int possibleCommandQtyInProc1Commands;
    public int possibleCommandQtyInProc2Commands;
    
    public List<BaseCommandSO> mainCommands = new List<BaseCommandSO>();
    public List<BaseCommandSO> proc1Commands = new List<BaseCommandSO>();
    public List<BaseCommandSO> proc2Commands = new List<BaseCommandSO>();

    public bool Contains(BaseCommandSO baseCommand)
    {
        return mainCommands.Contains(baseCommand) || proc1Commands.Contains(baseCommand) ||
               proc2Commands.Contains(baseCommand);
    }

    public bool UseColorCommands(ColorVariable color)
    {
        bool useColorCommands = false;
        foreach (BaseCommandSO baseCommand in mainCommands)
        {
            if (baseCommand.commandColor == color)
            {
                useColorCommands = true;
            }
        }
        foreach (BaseCommandSO baseCommand in proc1Commands)
        {
            if (baseCommand.commandColor == color)
            {
                useColorCommands = true;
            }
        }
        
        foreach (BaseCommandSO baseCommand in proc2Commands)
        {
            if (baseCommand.commandColor == color)
            {
                useColorCommands = true;
            }
        }

        if (useColorCommands)
        {
            Debug.Log("Current solution uses color commands");
        }
        else
        {
            Debug.Log("Current solution does NOT use color commands");
        }
        
        return useColorCommands;
    }
}
