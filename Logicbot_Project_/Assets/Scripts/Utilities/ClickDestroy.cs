using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ClickDestroy : MonoBehaviour
{
    public Action onDestroy;
    
    private Button _button;
        
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(DestroySelf);
    }

    public void DestroySelf()
    {
        onDestroy?.Invoke();
        Destroy(gameObject);
    }
}
