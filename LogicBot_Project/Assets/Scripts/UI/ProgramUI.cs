using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class ProgramUI : MonoBehaviour
{
    [SerializeField] private ProgramListCommandSO _programList;
    [SerializeField] private ReorderableList _reorderableList;
    [SerializeField] private RectTransform _listContent;

    [SerializeField] private ColorVariable _selectedColor;
    [SerializeField] private ColorVariable _deselectedColor;

    [SerializeField] private Image _titleBackgroundImage;

    [SerializeField] private GameEvent _reloadLevelGameEvent;

    [SerializeField] private Transform _backgroundTransform;

    [SerializeField] private Image _whiteBackgroundImagePrefab;

    private List<Image> _backgroundImages = new List<Image>();
    
    private void OnEnable()
    {
        _reloadLevelGameEvent.AddListener(OnReloadLevel);
    }

    private void Start()
    {
        _reorderableList.OnElementAdded.AddListener((args) =>
        {
            ClickDestroy clickDestroy = args.DroppedObject.GetComponent<ClickDestroy>();
            if (clickDestroy == null)
            {
                args.DroppedObject.gameObject.AddComponent<ClickDestroy>();
            }
            
            ClickAddCommandToSelectedProgram clickAddCommandToSelectedProgram =
                args.DroppedObject.GetComponent<ClickAddCommandToSelectedProgram>();
            if (clickAddCommandToSelectedProgram != null)
            {
                Debug.Log("Destroyng ClickAddCommandToSelectedProgram component");
                Destroy(clickAddCommandToSelectedProgram);
            }
        });
    }
    
    public void UpdateProgramList()
    {
        Debug.Log("Update " + name);
        _programList.ClearCommands();
        foreach (Transform commandUITransform in _listContent.transform)
        {
            if (commandUITransform == _listContent.transform) continue;
            CommandUI commandUI = commandUITransform.GetComponent<CommandUI>();
            if (commandUI != null)
            {
                _programList.Add(commandUI.GetCommand());
            }
        }
    }

    public void AddCommandUI(CommandUI commandUI)
    {
        if (_reorderableList.IsFull())
        {
            return;
        }
        
        CommandUI newCommandUI = Instantiate(commandUI, _listContent); // Maybe instantiate it in another place
        ClickDestroy clickDestroy = newCommandUI.GetComponent<ClickDestroy>();
        if (clickDestroy == null)
        {
            newCommandUI.gameObject.AddComponent<ClickDestroy>();
        }

        ClickAddCommandToSelectedProgram clickAddCommandToSelectedProgram =
            newCommandUI.GetComponent<ClickAddCommandToSelectedProgram>();
        if (clickAddCommandToSelectedProgram != null)
        {
            Debug.Log("Destroyng ClickAddCommandToSelectedProgram component");
            Destroy(clickAddCommandToSelectedProgram);
        }
        
        _reorderableList.Refresh();
    }

    public void SetAsSelected()
    {
        Debug.Log(name + "selected");
        _titleBackgroundImage.color = _selectedColor.Value;
        
        foreach (Image backgroundImage in _backgroundImages)
        {
            backgroundImage.color =_selectedColor.Value;
        }
        
    }

    public void SetAsDeselected()
    {
        Debug.Log(name + "deselected");
        _titleBackgroundImage.color = _deselectedColor.Value;
        foreach (Image backgroundImage in _backgroundImages)
        {
            backgroundImage.color = _deselectedColor.Value;
        }
    }

    private void OnReloadLevel()
    {
        ClearProgramListUI();
    }

    public void ClearProgramListUI()
    {
        foreach (Transform commandUITransform in _listContent.transform)
        {
            if (commandUITransform == _listContent.transform) continue;
            CommandUI commandUI = commandUITransform.GetComponent<CommandUI>();
            if (commandUI != null)
            {
                Destroy(commandUI.gameObject);
            }
        }
    }

    private void OnDisable()
    {
        _reloadLevelGameEvent.RemoveListener(OnReloadLevel);
    }

    private void DestroyAllBackgroundImages()
    {
        foreach (Image backgroundImage in _backgroundImages)
        {
            _backgroundImages.Remove(backgroundImage);
            Destroy(backgroundImage.gameObject);
        }
    }
    
    
    public void SetCommandsLimitTo(int maxItems)
    {
        _reorderableList.maxItems = maxItems;
        DestroyAllBackgroundImages();
        CreateBackgroundImages(maxItems);
    }

    private void CreateBackgroundImages(int maxItems)
    {
        for (int i = 0; i < maxItems; i++)
        {
            Image newBackgroundImage = Instantiate(_whiteBackgroundImagePrefab, _backgroundTransform);
            _backgroundImages.Add(newBackgroundImage);
        }
    }
}
