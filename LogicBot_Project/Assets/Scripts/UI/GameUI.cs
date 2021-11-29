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
    [SerializeField] private VerticalLayoutGroup _programVerticalLayoutGroup;
    [SerializeField] private GameObject _levelCompletedPanel;
    
    
    [Header("Variables")]
    [SerializeField] private BoolVariable _stopped;
    [SerializeField] private BoolVariable _isLevelCompleted;
    [SerializeField] private BoolVariable _isLoadingLevel;
    
    [Header("Events")]
    [SerializeField] private GameEvent _resetLevelGameEvent;
    [SerializeField] private GameEvent _reloadLevelGameEvent;
    [SerializeField] private GameEvent _loadNextLevelEvent;
    [SerializeField] private IntGameEvent _loadLevelEvent;
    [SerializeField] private GameEvent _levelCompletedGameEvent;
    [SerializeField] private GameEvent _onFinishedExecutionGameEvent;
    [SerializeField] private LevelDataGameEvent _setCurrentLevelDataGameEvent;

    private LevelDataSO _currentLevelData;
    private ProgramUI _selectedProgramUI;

    protected override void Awake()
    {
        base.Awake();
        _setCurrentLevelDataGameEvent.AddListener(SetCurrentLevelData);
        DisableAllProgramListUI();
        _isLoadingLevel.AddListener(UpdateUI);
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
        _loadLevelEvent.AddListener(OnLoadLevel);
        DeselectAllProgramUI();
        _selectedProgramUI = _mainProgramUI;
        _selectedProgramUI.SetAsSelected();
    }

    private void OnLoadLevel(int leveIndex)
    {
        ShowPlayButton();
        _levelCompletedPanel.SetActive(false);
    }

    private void OnLoadNextLevel()
    {
        ShowPlayButton();
        _levelCompletedPanel.SetActive(false);
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
        if (_proc1UI != null)
        {
            _proc1UI.SetAsDeselected();    
        }

        if (_proc2UI != null)
        {
            _proc2UI.SetAsDeselected();    
        }
    }

    private void DisableAllProgramListUI()
    {
        _mainProgramUI.gameObject.SetActive(false);
        _proc1UI.gameObject.SetActive(false);
        _proc2UI.gameObject.SetActive(false);
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
            if (_selectedProgramUI.isActiveAndEnabled)
            {
                _selectedProgramUI.AddCommandUI(commandUI);    
            }
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
        if (_isLoadingLevel.Value || _currentLevelData == null)
        {
            DisableAllProgramListUI();
            return;
        }

        ClearCommandProgramLists();
        
        _mainProgramUI.gameObject.SetActive(true);
        
        if (_currentLevelData.solution.possibleCommandQtyInMainCommands >=
            _currentLevelData.solution.mainCommands.Count)
        {
            _mainProgramUI.SetCommandsLimitTo(_currentLevelData.solution.possibleCommandQtyInMainCommands);
        }
        else
        {
            Debug.LogWarning("Possible command quantity in MAIN is less than solution");
            _mainProgramUI.SetCommandsLimitTo(_currentLevelData.solution.mainCommands.Count);    
        }

        _mainProgramUI.SetAsSelected();
        
        if (_proc1UI != null)
        {
            if (_currentLevelData.solution.proc1Commands.Count == 0 
                && _currentLevelData.solution.possibleCommandQtyInProc1Commands == 0)
            {
                _proc1UI.gameObject.SetActive(false);
            }
            else
            {
                _proc1UI.gameObject.SetActive(true);
                if (_currentLevelData.solution.possibleCommandQtyInProc1Commands >=
                    _currentLevelData.solution.proc1Commands.Count)
                {
                    _proc1UI.SetCommandsLimitTo(_currentLevelData.solution.possibleCommandQtyInProc1Commands);
                }
                else
                {
                    Debug.LogWarning("Possible command quantity in PROC 1 is less than solution");
                    _proc1UI.SetCommandsLimitTo(_currentLevelData.solution.proc1Commands.Count);    
                }
                
            }
        }

        if (_proc2UI != null)
        {
            if (_currentLevelData.solution.proc2Commands.Count == 0 
                && _currentLevelData.solution.possibleCommandQtyInProc2Commands == 0)
            {
                _proc2UI.gameObject.SetActive(false);
            }
            else
            {
                _proc2UI.gameObject.SetActive(true);
                
                
                if (_currentLevelData.solution.possibleCommandQtyInProc2Commands >=
                    _currentLevelData.solution.proc2Commands.Count)
                {
                    _proc2UI.SetCommandsLimitTo(_currentLevelData.solution.possibleCommandQtyInProc2Commands);
                }
                else
                {
                    Debug.LogWarning("Possible command quantity in PROC 2 is less than solution");
                    _proc2UI.SetCommandsLimitTo(_currentLevelData.solution.proc2Commands.Count);    
                }
            }  
        }

        // Quick fix to trigger layout rebuild
        this.Wait(.01f, () =>
        {
            LayoutRebuilder.MarkLayoutForRebuild (_programVerticalLayoutGroup.transform as RectTransform);
            SelectMainProgramUI();
        });
    }

    private void ClearCommandProgramLists()
    {
        _mainProgramUI.ClearProgramListUI();
        if (_proc1UI != null)
        {
            _proc1UI.ClearProgramListUI();    
        }

        if (_proc2UI != null)
        {
            _proc2UI.ClearProgramListUI();    
        }
    }
    

    private void OnDisable()
    {
        _onFinishedExecutionGameEvent.RemoveListener(OnFinishedExecutionHandler);
        _reloadLevelGameEvent.RemoveListener(OnReloadLevel);
        _levelCompletedGameEvent.RemoveListener(OnLevelCompleted);
        _loadNextLevelEvent.RemoveListener(OnLoadNextLevel);
        _loadLevelEvent.RemoveListener(OnLoadLevel);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _isLoadingLevel.RemoveListener(UpdateUI);
        _setCurrentLevelDataGameEvent.RemoveListener(SetCurrentLevelData);
    }
}
