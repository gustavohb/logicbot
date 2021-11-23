using System.Collections.Generic;
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
}
