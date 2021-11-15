using UnityEngine;

public abstract class ProgramUI : MonoBehaviour
{
    [SerializeField] private ProgramListCommandSO _programList;

    [SerializeField] private RectTransform _listContent;
    
    public virtual void UpdateProgramList()
    {
        Debug.Log("Update program list");
    }
}
