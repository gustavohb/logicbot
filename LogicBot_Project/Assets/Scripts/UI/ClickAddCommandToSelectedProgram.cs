using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(CommandUI))]
public class ClickAddCommandToSelectedProgram : MonoBehaviour
{
    private Button _button;
    private CommandUI _commandUI;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _commandUI = GetComponent<CommandUI>();
        
        _button.onClick.AddListener(AddCommandUIToSelectedProgramUI);
    }

    private void AddCommandUIToSelectedProgramUI()
    {
        if (_commandUI == null)
        {
            Debug.LogError("Command UI is null!");
        }
        else
        {
            GameUI.instance.AddCommandUIToSelectedProgramUI(_commandUI);    
        }
    }
}
