using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ReloadButtonUI : MonoBehaviour
{
    [SerializeField] private BoolVariable _stopped;
    [SerializeField] private GameEvent _reloadLevelGameEvent;
    
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ReloadLevel);
        _stopped.AddListener(UpdateUI);
    }

    private void ReloadLevel()
    {
        if (_stopped.Value)
        {
            _reloadLevelGameEvent.Raise();
        }
    }
    
    private void UpdateUI()
    {
        _button.interactable = _stopped.Value;
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(ReloadLevel);
        _stopped.RemoveListener(UpdateUI);
    }
}
