using UnityEngine;
using UnityEngine.UI;

public class TestGameUI : MonoBehaviour
{
    [SerializeField] private Button _playButton;

    [SerializeField] private ProgramUI _mainProgramUI;
    [SerializeField] private ProgramUI _proc1UI;
    [SerializeField] private ProgramUI _proc2UI;
    
    private void Start()
    {
        _playButton.onClick.AddListener(ExecuteCommands);
    }
    
    public void ExecuteCommands()
    {
        _proc1UI.UpdateProgramList();
        _proc2UI.UpdateProgramList();
        _mainProgramUI.UpdateProgramList();
        
        CallCommandProcessorToExecuteCommands();
    }

    private void CallCommandProcessorToExecuteCommands()
    {
        CommandProcessor.instance.ExecuteCommands();
    }
}
