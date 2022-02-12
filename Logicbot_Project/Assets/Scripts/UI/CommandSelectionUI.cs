using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TabGroupUI))]
public class CommandSelectionUI : MonoBehaviour
{
    [Header("Default Command Buttons")]
    [SerializeField] private Button _moveForwardButtonDefault;
    [SerializeField] private Button _turnLeftButtonDefault;
    [SerializeField] private Button _turnRightButtonDefault;
    [SerializeField] private Button _jumpButtonDefault;
    [SerializeField] private Button _turnLightOnButtonDefault;
    [SerializeField] private Button _proc1ButtonDefault;
    [SerializeField] private Button _proc2ButtonDefault;
    [SerializeField] private Button _breakButtonDefault;
    
    [Header("Green Command Buttons")]
    [SerializeField] private Button _moveForwardButtonGreen;
    [SerializeField] private Button _turnLeftButtonGreen;
    [SerializeField] private Button _turnRightButtonGreen;
    [SerializeField] private Button _jumpButtonGreen;
    [SerializeField] private Button _turnLightOnButtonGreen;
    [SerializeField] private Button _proc1ButtonGreen;
    [SerializeField] private Button _proc2ButtonGreen;
    [SerializeField] private Button _breakButtonGreen;
    
    [Header("Pink Command Buttons")]
    [SerializeField] private Button _moveForwardButtonPink;
    [SerializeField] private Button _turnLeftButtonPink;
    [SerializeField] private Button _turnRightButtonPink;
    [SerializeField] private Button _jumpButtonPink;
    [SerializeField] private Button _turnLightOnButtonPink;
    [SerializeField] private Button _proc1ButtonPink;
    [SerializeField] private Button _proc2ButtonPink;
    [SerializeField] private Button _breakButtonPink;
    
    [Header("Default Commands")]
    [SerializeField] private BaseCommandSO _moveForwardCommandDefault;
    [SerializeField] private BaseCommandSO _turnLeftCommandDefault;
    [SerializeField] private BaseCommandSO _turnRightCommandDefault;
    [SerializeField] private BaseCommandSO _jumpCommandDefault;
    [SerializeField] private BaseCommandSO _turnLightOnCommandDefault;
    [SerializeField] private BaseCommandSO _proc1CommandDefault;
    [SerializeField] private BaseCommandSO _proc2CommandDefault;
    
    [Header("Break Commands")]
    [SerializeField] private BaseCommandSO _breakCommandDefault;
    [SerializeField] private BaseCommandSO _breakCommandGreen;
    [SerializeField] private BaseCommandSO _breakCommandPink;
    
    [Header("Change Color Tab Buttons")] 
    [SerializeField] private GameObject _greenPaintBrushTabButton;
    [SerializeField] private GameObject _pinkPaintBrushTabButton;
    
    [Header("Color Variables")] 
    [SerializeField] private ColorVariable _greenCommandColor;
    [SerializeField] private ColorVariable _pinkCommandColor;
    
    [Header("Events")]
    [SerializeField] private LevelDataGameEvent _setCurrentLevelDataGameEvent;

    private TabGroupUI _tabGroup;
    private LevelSolutionSO _currentLevelSolution;
    private LevelDataSO _currentLevelData;
    
    private void Awake()
    {
        _tabGroup = GetComponent<TabGroupUI>();
        _setCurrentLevelDataGameEvent.AddListener(SetCurrentLevelData);
    }

    private void SetCurrentLevelData(LevelDataSO levelData)
    {
        _currentLevelData = levelData;
        _currentLevelSolution = _currentLevelData.solution;
        UpdateUI();
    }

    private void UpdateUI()
    {
        DisableAllButtons();
        
        _tabGroup.ResetTabs(true);
        
        bool useGreenCommands = _currentLevelSolution.UseColorCommands(_greenCommandColor);
        bool usePinkCommands = _currentLevelSolution.UseColorCommands(_pinkCommandColor);

        if (_currentLevelSolution.Contains(_breakCommandDefault)
            || _currentLevelSolution.Contains(_breakCommandGreen)
            || _currentLevelSolution.Contains(_breakCommandPink))
        {
            _breakButtonDefault.gameObject.SetActive(true);
            _breakButtonGreen.gameObject.SetActive(true);
            _breakButtonPink.gameObject.SetActive(true);
        }
        
        if (useGreenCommands || usePinkCommands)
        {
            EnableAllButtonsButBreakOnes();
            if (useGreenCommands)
            {
                _greenPaintBrushTabButton.SetActive(true);
            }
            if (usePinkCommands)
            {
                _pinkPaintBrushTabButton.SetActive(true);
            }
        }

        // Move forward - always appear
        _moveForwardButtonDefault.gameObject.SetActive(true);
        
        // Turn light on - always appear
        _turnLightOnButtonDefault.gameObject.SetActive(true);
        
        // Turns
        if (_currentLevelSolution.mainCommands.Contains(_turnLeftCommandDefault) ||
            _currentLevelSolution.proc1Commands.Contains(_turnLeftCommandDefault) ||
            _currentLevelSolution.proc2Commands.Contains(_turnLeftCommandDefault) ||
            _currentLevelSolution.mainCommands.Contains(_turnRightCommandDefault) ||
            _currentLevelSolution.proc1Commands.Contains(_turnRightCommandDefault) ||
            _currentLevelSolution.proc2Commands.Contains(_turnRightCommandDefault))
        {
            EnableDefaultTurnButtons();
        }
        
        // Jump
        if (_currentLevelSolution.mainCommands.Contains(_jumpCommandDefault) ||
            _currentLevelSolution.proc1Commands.Contains(_jumpCommandDefault) ||
            _currentLevelSolution.proc2Commands.Contains(_jumpCommandDefault))
        {
            EnableDefaultTurnButtons();
            _jumpButtonDefault.gameObject.SetActive(true);
        }
        
        // Proc1
        if (_currentLevelSolution.proc1Commands.Count > 0 || _currentLevelSolution.possibleCommandQtyInProc1Commands > 0)
        {
            EnableAllDefaultMovementButtons();
            _proc1ButtonDefault.gameObject.SetActive(true);
        }
        else
        {
            DisableAllProc1Buttons();
            DisableAllProc2Buttons();
        }
        
        // Proc2
        if (_currentLevelSolution.proc2Commands.Count > 0 || _currentLevelSolution.possibleCommandQtyInProc2Commands > 0)
        {
            EnableAllDefaultMovementButtons();
            _proc2ButtonDefault.gameObject.SetActive(true);
        }
        else
        {
            DisableAllProc2Buttons();
        }
    }


    private void EnableDefaultTurnButtons()
    {
        _turnLeftButtonDefault.gameObject.SetActive(true);
        _turnRightButtonDefault.gameObject.SetActive(true);
    }
    
    
    
    private void EnableAllDefaultMovementButtons()
    {
        // Default
        _moveForwardButtonDefault.gameObject.SetActive(true);
        _turnLeftButtonDefault.gameObject.SetActive(true);
        _turnRightButtonDefault.gameObject.SetActive(true);
        _jumpButtonDefault.gameObject.SetActive(true);
        _turnLightOnButtonDefault.gameObject.SetActive(true);
        //_proc1ButtonDefault.gameObject.SetActive(true);
        //_proc2ButtonDefault.gameObject.SetActive(true);
    }

    private void EnableAllGreenMovementButtons()
    {
        // Green
        _moveForwardButtonGreen.gameObject.SetActive(true);
        _turnLeftButtonGreen.gameObject.SetActive(true);
        _turnRightButtonGreen.gameObject.SetActive(true);
        _jumpButtonGreen.gameObject.SetActive(true);
        _turnLightOnButtonGreen.gameObject.SetActive(true);
        //_proc1ButtonGreen.gameObject.SetActive(true);
        //_proc2ButtonGreen.gameObject.SetActive(true);
    }

    private void EnableAllPinkMovementButtons()
    {
        // Pink
        _moveForwardButtonPink.gameObject.SetActive(true);
        _turnLeftButtonPink.gameObject.SetActive(true);
        _turnRightButtonPink.gameObject.SetActive(true);
        _jumpButtonPink.gameObject.SetActive(true);
        _turnLightOnButtonPink.gameObject.SetActive(true);
        //_proc1ButtonPink.gameObject.SetActive(true);
        //_proc2ButtonPink.gameObject.SetActive(true);
    }

    private void DisableAllButtons()
    {
        _greenPaintBrushTabButton.SetActive(false);
        _pinkPaintBrushTabButton.SetActive(false);
        
        _moveForwardButtonDefault.gameObject.SetActive(false);
        _turnLeftButtonDefault.gameObject.SetActive(false);
        _turnRightButtonDefault.gameObject.SetActive(false);
        _jumpButtonDefault.gameObject.SetActive(false);
        _turnLightOnButtonDefault.gameObject.SetActive(false);
        _proc1ButtonDefault.gameObject.SetActive(false);
        _proc2ButtonDefault.gameObject.SetActive(false);
        
        // Break buttons
        _breakButtonDefault.gameObject.SetActive(false);
        _breakButtonGreen.gameObject.SetActive(false);
        _breakButtonPink.gameObject.SetActive(false);
    }

    private void EnableAllButtonsButBreakOnes()
    {
        // Default
        EnableAllDefaultMovementButtons();
        
        // Green
        EnableAllGreenMovementButtons();
        
        
        // Pink
        EnableAllPinkMovementButtons();
        
        // PROC1
        EnableAllProc1Buttons();
        
        // PROC2
        EnableAllProc2Buttons();
    }

    private void EnableAllProc1Buttons()
    {
        _proc1ButtonDefault.gameObject.SetActive(true);
        _proc1ButtonGreen.gameObject.SetActive(true);
        _proc1ButtonPink.gameObject.SetActive(true);
    }
    
    private void DisableAllProc1Buttons()
    {
        _proc1ButtonDefault.gameObject.SetActive(false);
        _proc1ButtonGreen.gameObject.SetActive(false);
        _proc1ButtonPink.gameObject.SetActive(false);
    }

    private void EnableAllProc2Buttons()
    {
        _proc2ButtonDefault.gameObject.SetActive(true);
        _proc2ButtonGreen.gameObject.SetActive(true);
        _proc2ButtonPink.gameObject.SetActive(true);
    }

    private void DisableAllProc2Buttons()
    {
        _proc2ButtonDefault.gameObject.SetActive(false);
        _proc2ButtonGreen.gameObject.SetActive(false);
        _proc2ButtonPink.gameObject.SetActive(false);
    }
    
}
