using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CommandUI : MonoBehaviour
{
    [SerializeField] private bool _isProgramList = false;
    [SerializeField] private BaseCommandSO _baseCommand;

    [SerializeField] private Image _image;

    [SerializeField] private ColorVariable _executingColor;

    private Color _originalColor;
    
    private void Awake()
    {
        if (_image == null)
        {
            _image = GetComponent<Image>();    
        }

        _originalColor = _image.color;
    }

    public BaseCommandSO GetCommand()
    {
        //if (!_isProgramList)
        //{
            BaseCommandSO newBaseCommand = ScriptableObject.Instantiate(_baseCommand); 
            newBaseCommand.SetCommandUI(this);
            return newBaseCommand;
        //}

        //return _baseCommand;
    }

    public void SetAsExecuting()
    {
        if (_image != null)
        {
            _image.color = _executingColor.Value;
        }
    }

    public void SetAsNotExecuting()
    {
        if (_image != null)
        {
            _image.color = _originalColor;
        }
    }
    
}
