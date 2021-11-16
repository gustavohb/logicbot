using UnityEngine;
using UnityEngine.UI.Extensions;

public class ProgramUI : MonoBehaviour
{
    [SerializeField] private ProgramListCommandSO _programList;
    [SerializeField] private ReorderableList _reorderableList;
    [SerializeField] private RectTransform _listContent;

    
    private void Start()
    {
        _reorderableList.OnElementAdded.AddListener((args) =>
        {
            ClickDestroy clickDestroy = args.DroppedObject.GetComponent<ClickDestroy>();
            if (clickDestroy == null)
            {
                args.DroppedObject.gameObject.AddComponent<ClickDestroy>();
            }
        });
    }
    
    public void UpdateProgramList()
    {
        Debug.Log("Update " + name);
        _programList.ClearCommands();
        foreach (Transform commandUITransform in _listContent.transform)
        {
            if (commandUITransform == _listContent.transform) continue;
            CommandUI commandUI = commandUITransform.GetComponent<CommandUI>();
            if (commandUI != null)
            {
                _programList.Add(commandUI.GetCommand());
            }
        }
    }
}
