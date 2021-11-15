using UnityEngine;

public class CommandUI : MonoBehaviour
{
    [SerializeField] private bool _isProgramList = false;
    [SerializeField] private BaseCommandSO _baseCommand;

    public BaseCommandSO GetCommand()
    {
        if (!_isProgramList)
        {
            return ScriptableObject.Instantiate(_baseCommand);    
        }
        
        return _baseCommand;
    }
    
}
