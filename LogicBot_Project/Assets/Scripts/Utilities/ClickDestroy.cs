using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ClickDestroy : MonoBehaviour
{
    private Button _button;
        
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(DestroySelf);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
