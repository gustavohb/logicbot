using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ClickGameEvent : MonoBehaviour
{
    [SerializeField] private GameEvent _gameEvent;

    private Button _button;
        
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(RaiseEvent);
    }

    public void RaiseEvent()
    {
        _gameEvent.Raise();
    }
}    
