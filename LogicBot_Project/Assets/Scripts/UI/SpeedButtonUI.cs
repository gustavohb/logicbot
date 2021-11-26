using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SpeedButtonUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _normalSpeedIcon;
    [SerializeField] private Image _fastSpeedIcon;
    
    [Header("Variables")]
    [SerializeField] private FloatVariable _fastSpeedCommandExecutionDuration;
    [SerializeField] private FloatVariable _normalSpeedCommandExecutionDuration;
    [SerializeField] private BoolVariable _isFastModeEnabled;
    
    [Header("Events")]
    [SerializeField] private FloatGameEvent _setCommandDurationEvent;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ToggleSpeed);
        
        UpdateUI();
    }

    private void ToggleSpeed()
    {
        _isFastModeEnabled.Value = !_isFastModeEnabled.Value;

        _setCommandDurationEvent.Raise(_isFastModeEnabled.Value
            ? _fastSpeedCommandExecutionDuration.Value
            : _normalSpeedCommandExecutionDuration.Value);

        UpdateUI();
    }

    private void UpdateUI()
    {
        _fastSpeedIcon.gameObject.SetActive(_isFastModeEnabled.Value);
        _normalSpeedIcon.gameObject.SetActive(!_isFastModeEnabled.Value);
    }
}
