using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _stopButton;
    [SerializeField] private Button _rewindButton;
    
    [SerializeField] private ProgramUI _mainProgramUI;
    [SerializeField] private ProgramUI _proc1UI;
    [SerializeField] private ProgramUI _proc2UI;
    
    [SerializeField] private BoolVariable _stopped;

    [SerializeField] private GameEvent _resetLevelGameEvent;
    [SerializeField] private GameEvent _reloadLevelGameEvent;
    [SerializeField] private GameEvent _onFinishedExecutionGameEvent;

    private ProgramUI _selectedProgramUI;

    private void OnEnable()
    {
        _onFinishedExecutionGameEvent.AddListener(OnFinishedExecutionHandler);
        _reloadLevelGameEvent.AddListener(OnReloadLevel);
        DeselectAllProgramUI();
        _selectedProgramUI = _mainProgramUI;
        _selectedProgramUI.SetAsSelected();
    }
    
    private void Start()
    {
        _playButton.onClick.AddListener(ExecuteCommands);
        _stopButton.onClick.AddListener(StopExecution);
        _rewindButton.onClick.AddListener(RewindExecution);
        DisableAllButtons();
        _playButton.gameObject.SetActive(true);
    }

    public void SelectMainProgramUI()
    {
        DeselectAllProgramUI();
        _selectedProgramUI = _mainProgramUI;
        _selectedProgramUI.SetAsSelected();
    }

    public void SelectProc1UI()
    {
        DeselectAllProgramUI();
        _selectedProgramUI = _proc1UI;
        _selectedProgramUI.SetAsSelected();
    }
    
    public void SelectProc2UI()
    {
        DeselectAllProgramUI();
        _selectedProgramUI = _proc2UI;
        _selectedProgramUI.SetAsSelected();
    }
    
    private void DeselectAllProgramUI()
    {
        _mainProgramUI.SetAsDeselected();
        _proc1UI.SetAsDeselected();
        _proc2UI.SetAsDeselected();
    }

    private void OnFinishedExecutionHandler()
    {
        _stopped.Value = true;
        DisableAllButtons();
        _rewindButton.gameObject.SetActive(true);
    }

    

    private void DisableAllButtons()
    {
        _playButton.gameObject.SetActive(false);
        _stopButton.gameObject.SetActive(false);
        _rewindButton.gameObject.SetActive(false);
    }

    public void AddCommandUIToSelectedProgramUI(CommandUI commandUI)
    {
        _selectedProgramUI.AddCommandUI(commandUI);
    }
    
    public void StopExecution()
    {
        _stopped.Value = true;
        DisableAllButtons();
        _playButton.gameObject.SetActive(true);
    }

    public void RewindExecution()
    {
        DisableAllButtons();
        _stopped.Value = true;
        _resetLevelGameEvent.Raise();
        _playButton.gameObject.SetActive(true);
    }
    
    public void ExecuteCommands()
    {
        _proc1UI.UpdateProgramList();
        _proc2UI.UpdateProgramList();
        _mainProgramUI.UpdateProgramList();

        DisableAllButtons();
        _stopButton.gameObject.SetActive(true);
        
        CallCommandProcessorToExecuteCommands();
    }

    private void OnReloadLevel()
    {
        DisableAllButtons();
        _playButton.gameObject.SetActive(true);
    }
    
    private void CallCommandProcessorToExecuteCommands()
    {
        CommandProcessor.instance.ExecuteCommands();
    }

    private void OnDisable()
    {
        _onFinishedExecutionGameEvent.RemoveListener(OnFinishedExecutionHandler);
        _reloadLevelGameEvent.RemoveListener(OnReloadLevel);
    }
}
