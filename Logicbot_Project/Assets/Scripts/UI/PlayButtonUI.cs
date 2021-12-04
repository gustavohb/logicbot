using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayButtonUI : MonoBehaviour
{
    [SerializeField] private BoolVariable _doesMainProgramHaveCommands;
    [SerializeField] private BoolVariable _isLoadingLevel;
    
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _doesMainProgramHaveCommands.AddListener(UpdateUI);
        _isLoadingLevel.AddListener(UpdateUI);
    }

    private void UpdateUI()
    {
        _button.interactable = _doesMainProgramHaveCommands.Value && !_isLoadingLevel.Value;
    }

    private void OnDestroy()
    {
        _doesMainProgramHaveCommands.RemoveListener(UpdateUI);
        _isLoadingLevel.RemoveListener(UpdateUI);
    }
}
