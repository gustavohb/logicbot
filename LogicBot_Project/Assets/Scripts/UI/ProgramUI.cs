using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class ProgramUI : MonoBehaviour
{
    [SerializeField] private ProgramListCommandSO _programList;
    [SerializeField] private ReorderableList _reorderableList;
    [SerializeField] private RectTransform _listContent;

    [SerializeField] private ColorVariable _selectedColor;
    [SerializeField] private ColorVariable _deselectedColor;

    [SerializeField] private Image _titleBackgroundImage;
    [SerializeField] private Image _backgroundImage;
    
    private void Start()
    {
        _reorderableList.OnElementAdded.AddListener((args) =>
        {
            ClickDestroy clickDestroy = args.DroppedObject.GetComponent<ClickDestroy>();
            if (clickDestroy == null)
            {
                args.DroppedObject.gameObject.AddComponent<ClickDestroy>();
            }
            
            ClickAddCommandToSelectedProgram clickAddCommandToSelectedProgram =
                args.DroppedObject.GetComponent<ClickAddCommandToSelectedProgram>();
            if (clickAddCommandToSelectedProgram != null)
            {
                Debug.Log("Destroyng ClickAddCommandToSelectedProgram component");
                Destroy(clickAddCommandToSelectedProgram);
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

    public void AddCommandUI(CommandUI commandUI)
    {
        CommandUI newCommandUI = Instantiate(commandUI, _listContent); // Maybe instantiate it in another place
        ClickDestroy clickDestroy = newCommandUI.GetComponent<ClickDestroy>();
        if (clickDestroy == null)
        {
            newCommandUI.gameObject.AddComponent<ClickDestroy>();
        }

        ClickAddCommandToSelectedProgram clickAddCommandToSelectedProgram =
            newCommandUI.GetComponent<ClickAddCommandToSelectedProgram>();
        if (clickAddCommandToSelectedProgram != null)
        {
            Debug.Log("Destroyng ClickAddCommandToSelectedProgram component");
            Destroy(clickAddCommandToSelectedProgram);
        }
        
        _reorderableList.Refresh();
    }

    public void SetAsSelected()
    {
        Debug.Log(name + "selected");
        _titleBackgroundImage.color = _selectedColor.Value;
        _backgroundImage.color = _selectedColor.Value;
    }

    public void SetAsDeselected()
    {
        Debug.Log(name + "deselected");
        _titleBackgroundImage.color = _deselectedColor.Value;
        _backgroundImage.color = _deselectedColor.Value;
    }
}
