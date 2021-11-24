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


    private LevelSolutionSO _currentLevelSolution;
    private LevelDataSO _currentLevelData;
    
    [Header("Events")]
    [SerializeField] private LevelDataGameEvent _setCurrentLevelDataGameEvent;
    
    private void Awake()
    {
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
        _moveForwardButton.gameObject.SetActive(true);
        
        // Turn light on - always appear
        _turnLightOnButton.gameObject.SetActive(true);
        
        // Turns
        if (_currentLevelSolution.mainCommands.Contains(_turnLeftCommand) ||
            _currentLevelSolution.proc1Commands.Contains(_turnLeftCommand) ||
            _currentLevelSolution.proc2Commands.Contains(_turnLeftCommand) ||
            _currentLevelSolution.mainCommands.Contains(_turnRightCommand) ||
            _currentLevelSolution.proc1Commands.Contains(_turnRightCommand) ||
            _currentLevelSolution.proc2Commands.Contains(_turnRightCommand))
        {
            _turnLeftButton.gameObject.SetActive(true);
            _turnRightButton.gameObject.SetActive(true);
        }
        
        // Jump
        if (_currentLevelSolution.mainCommands.Contains(_jumpCommand) ||
            _currentLevelSolution.proc1Commands.Contains(_jumpCommand) ||
            _currentLevelSolution.proc2Commands.Contains(_jumpCommand))
        {
            _jumpButton.gameObject.SetActive(true);
        }
        
        // Proc1
        if (_currentLevelSolution.proc1Commands.Count > 0)
        {
            _proc1Button.gameObject.SetActive(true);
        }
        
        // Proc2
        if (_currentLevelSolution.proc2Commands.Count > 0)
        {
            _proc2Button.gameObject.SetActive(true);
        }
    }
    
    
    private void DisableAllButtons()
    {
        _moveForwardButton.gameObject.SetActive(false);
        _turnLeftButton.gameObject.SetActive(false);
        _turnRightButton.gameObject.SetActive(false);
        _jumpButton.gameObject.SetActive(false);
        _turnLightOnButton.gameObject.SetActive(false);
        _proc1Button.gameObject.SetActive(false);
        _proc2Button.gameObject.SetActive(false);
    }
}
