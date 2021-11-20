using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    [Header("Buttons")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _stopButton;
    [SerializeField] private Button _rewindButton;
    
    
    [Header("Program UI")]
    [SerializeField] private ProgramUI _mainProgramUI;
    [SerializeField] private ProgramUI _proc1UI;
    [SerializeField] private ProgramUI _proc2UI;

    [Header("References")] 
    [SerializeField] private GameObject _levelCompletedPanel;
    
    
    [Header("Variables")]
    [SerializeField] private BoolVariable _stopped;
    [SerializeField] private BoolVariable _isLevelCompleted;
    
    [Header("Events")]
    [SerializeField] private GameEvent _resetLevelGameEvent;
    [SerializeField] private GameEvent _reloadLevelGameEvent;
    [SerializeField] private GameEvent _loadNextLevelEvent;
    [SerializeField] private GameEvent _levelCompletedGameEvent;
    [SerializeField] private GameEvent _onFinishedExecutionGameEvent;
    [SerializeField] private LevelDataGameEvent _setCurrentLevelDataGameEvent;

    private LevelDataSO _currentLevelData;
    private ProgramUI _selectedProgramUI;

    protected override void Awake()
    {
        base.Awake();
        _setCurrentLevelDataGameEvent.AddListener(SetCurrentLevelData);
    }

    private void SetCurrentLevelData(LevelDataSO levelData)
    {
        _currentLevelData = levelData;
        UpdateUI();
    }
    
    private void OnEnable()
    {
        _onFinishedExecutionGameEvent.AddListener(OnFinishedExecutionHandler);
        _reloadLevelGameEvent.AddListener(OnReloadLevel);
        _levelCompletedGameEvent.AddListener(OnLevelCompleted);
        _loadNextLevelEvent.AddListener(OnLoadNextLevel);
        DeselectAllProgramUI();
        _selectedProgramUI = _mainProgramUI;
        _selectedProgramUI.SetAsSelected();
    }

    private void OnLoadNextLevel()
    {
        ShowPlayButton();
    }

    private void ShowPlayButton()
    {
        DisableAllButtons();
        _playButton.gameObject.SetActive(true);
    }

    private void ShowRewindButton()
    {
        DisableAllButtons();
        if (!_isLevelCompleted.Value)
        {
            _rewindButton.gameObject.SetActive(true);    
        }
    }

    private void ShowStopButton()
    {
        DisableAllButtons();
        _stopButton.gameObject.SetActive(true);
    }
    
    private void OnLevelCompleted()
    {
        DisableAllButtons();
        _levelCompletedPanel.SetActive(true);
    }

    private void Start()
    {
        _playButton.onClick.AddListener(ExecuteCommands);
        _stopButton.onClick.AddListener(StopExecution);
        _rewindButton.onClick.AddListener(RewindExecution);
        ShowPlayButton();
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
        ShowRewindButton();
    }
    
    private void DisableAllButtons()
    {
        _playButton.gameObject.SetActive(false);
        _stopButton.gameObject.SetActive(false);
        _rewindButton.gameObject.SetActive(false);
    }

    public void AddCommandUIToSelectedProgramUI(CommandUI commandUI)
    {
        if (_selectedProgramUI == null)
        {
            Debug.LogError("Selected program UI is null!");
        }
        else
        {
            _selectedProgramUI.AddCommandUI(commandUI);    
        }
    }
    
    public void StopExecution()
    {
        _stopped.Value = true;
        ShowPlayButton();
    }

    public void RewindExecution()
    {
        _stopped.Value = true;
        _resetLevelGameEvent.Raise();
        ShowPlayButton();
    }
    
    public void ExecuteCommands()
    {
        _proc1UI.UpdateProgramList();
        _proc2UI.UpdateProgramList();
        _mainProgramUI.UpdateProgramList();

        ShowStopButton();
        
        CallCommandProcessorToExecuteCommands();
    }

    private void OnReloadLevel()
    {
        ShowPlayButton();
    }
    
    private void CallCommandProcessorToExecuteCommands()
    {
        CommandProcessor.instance.ExecuteCommands();
    }

    private void UpdateUI()
    {
        Debug.Log("Update UI'");
        ClearCommandProgramLists();
        _mainProgramUI.SetCommandsLimitTo(_currentLevelData.solution.mainCommands.Count);
        _mainProgramUI.SetAsSelected();
        _proc1UI.SetCommandsLimitTo(_currentLevelData.solution.proc1Commands.Count);
        _proc2UI.SetCommandsLimitTo(_currentLevelData.solution.proc2Commands.Count);
    }

    private void ClearCommandProgramLists()
    {
        _mainProgramUI.ClearProgramListUI();
        _proc1UI.ClearProgramListUI();
        _proc2UI.ClearProgramListUI();
    }
    

    private void OnDisable()
    {
        _onFinishedExecutionGameEvent.RemoveListener(OnFinishedExecutionHandler);
        _reloadLevelGameEvent.RemoveListener(OnReloadLevel);
        _levelCompletedGameEvent.RemoveListener(OnLevelCompleted);
        _loadNextLevelEvent.RemoveListener(OnLoadNextLevel);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _setCurrentLevelDataGameEvent.RemoveListener(SetCurrentLevelData);
    }
}
