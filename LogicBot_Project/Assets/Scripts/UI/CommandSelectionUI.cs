using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

public class CommandSelectionUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _moveForwardButton;
    [SerializeField] private Button _turnLeftButton;
    [SerializeField] private Button _turnRightButton;
    [SerializeField] private Button _jumpButton;
    [SerializeField] private Button _turnLightOnButton;
    [SerializeField] private Button _proc1Button;
    [SerializeField] private Button _proc2Button;

    [Header("Commands")]
    [SerializeField] private BaseCommandSO _moveForwardCommand;
    [SerializeField] private BaseCommandSO _turnLeftCommand;
    [SerializeField] private BaseCommandSO _turnRightCommand;
    [SerializeField] private BaseCommandSO _jumpCommand;
    [SerializeField] private BaseCommandSO _turnLightOnCommand;
    [SerializeField] private BaseCommandSO _proc1Command;
    [SerializeField] private BaseCommandSO _proc2Command;
    
    private GameObject _moveForwardButtonGO;
    private GameObject _turnLeftButtonGO;
    private GameObject _turnRightButtonGO;
    private GameObject _jumpButtonGO;
    private GameObject _turnLightOnButtonGO;
    private GameObject _proc1ButtonGO;
    private GameObject _proc2ButtonGO;

    private LevelSolutionSO _currentLevelSolution;
    private LevelDataSO _currentLevelData;
    
    [Header("Events")]
    [SerializeField] private LevelDataGameEvent _setCurrentLevelDataGameEvent;
    
    private void Awake()
    {
        _moveForwardButtonGO = _moveForwardButton.gameObject;
        _turnLeftButtonGO = _turnLeftButton.gameObject;
        _turnRightButtonGO = _turnRightButton.gameObject;
        _jumpButtonGO = _jumpButton.gameObject;
        _turnLightOnButtonGO = _turnLightOnButton.gameObject;
        _proc1ButtonGO = _proc1Button.gameObject;
        _proc2ButtonGO = _proc2Button.gameObject;
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
        // Move forward - always appear
        _moveForwardButtonGO.SetActive(true);
        
        // Turn light on - always appear
        _turnLightOnButtonGO.SetActive(true);
        
        // Turns
        if (_currentLevelSolution.mainCommands.Contains(_turnLeftCommand) ||
            _currentLevelSolution.proc1Commands.Contains(_turnLeftCommand) ||
            _currentLevelSolution.proc2Commands.Contains(_turnLeftCommand) ||
            _currentLevelSolution.mainCommands.Contains(_turnRightCommand) ||
            _currentLevelSolution.proc1Commands.Contains(_turnRightCommand) ||
            _currentLevelSolution.proc2Commands.Contains(_turnRightCommand))
        {
            _turnLeftButtonGO.SetActive(true);
            _turnRightButtonGO.SetActive(true);
        }
        
        // Jump
        if (_currentLevelSolution.mainCommands.Contains(_jumpCommand) ||
            _currentLevelSolution.proc1Commands.Contains(_jumpCommand) ||
            _currentLevelSolution.proc2Commands.Contains(_jumpCommand))
        {
            _jumpButtonGO.SetActive(true);
        }
        
        // Proc1
        if (_currentLevelSolution.proc1Commands.Count > 0)
        {
            _proc1ButtonGO.SetActive(true);
        }
        
        // Proc2
        if (_currentLevelSolution.proc2Commands.Count > 0)
        {
            _proc2ButtonGO.SetActive(true);
        }
    }
    
    
    private void DisableAllButtons()
    {
        _moveForwardButtonGO.SetActive(false);
        _turnLeftButtonGO.SetActive(false);
        _turnRightButtonGO.SetActive(false);
        _jumpButtonGO.SetActive(false);
        _turnLightOnButtonGO.SetActive(false);
        _proc1ButtonGO.SetActive(false);
        _proc2ButtonGO.SetActive(false);
    }
}
