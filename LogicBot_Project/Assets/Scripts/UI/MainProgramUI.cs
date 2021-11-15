using UnityEngine;

public class MainProgramUI : ProgramUI
{
    [SerializeField] private ProgramListCommandSO _programList;
    [SerializeField] private RectTransform _listContent;
    
    public override void UpdateProgramList()
    {
        Debug.Log("Update main program ui");
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
