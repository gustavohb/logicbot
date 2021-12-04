using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Commands/CommandList")]
public class CommandListSO : ScriptableObject
{

    public List<BaseCommandSO> commandList = new List<BaseCommandSO>();

    public BaseCommandSO this[int i]
    {
        get
        {
            return commandList[i];
        }
    }
    
    public int Count
    {
        get => commandList != null ? commandList.Count : 0;
    }
    
    public void Add(BaseCommandSO baseCommand)
    {
        commandList.Add(baseCommand);
    }
    
    public void Clear()
    {
        commandList.Clear();
    }
    
    
    

}
