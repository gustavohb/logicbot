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

    [SerializeField] private BoolVariable _hasValidProgram;

    [SerializeField] private BoolVariable _isLoadingLevel;
    
    [SerializeField] private Image _titleBackgroundImage;

    [SerializeField] private GameEvent _reloadLevelGameEvent;

    [SerializeField] private Transform _backgroundTransform;

    [SerializeField] private Image _whiteBackgroundImagePrefab;

    
    
    private RectTransform _reordableListRectTransform;
    
    private List<Image> _backgroundImages = new List<Image>();
    
    private void OnEnable()
    {
        _reloadLevelGameEvent.AddListener(OnReloadLevel);
        _reordableListRectTransform = _reorderableList.GetComponent<RectTransform>();
    }

    private void Start()
    {
        if (_hasValidProgram != null)
        {
            _hasValidProgram.Value = false;
            _hasValidProgram.Raise();
        }
        
        _reorderableList.OnElementAdded.AddListener((args) =>
        {

            if (_hasValidProgram != null)
            {
                _hasValidProgram.Value = true;
            }
            
            ClickDestroy clickDestroy = args.DroppedObject.GetComponent<ClickDestroy>();
            if (clickDestroy == null)
            {
                clickDestroy = args.DroppedObject.gameObject.AddComponent<ClickDestroy>();
            }
            else
            {
                clickDestroy.onDestroy = null;
            }

            clickDestroy.onDestroy += () =>
            {
                if (_hasValidProgram != null)
                {
                    if (_reorderableList.Content.childCount <= 1)
                    {
                        _hasValidProgram.Value = false;
                    }
                    else
                    {
                        _hasValidProgram.Value = true;
                    }
                }
            };
            
            
            ClickAddCommandToSelectedProgram clickAddCommandToSelectedProgram =
                args.DroppedObject.GetComponent<ClickAddCommandToSelectedProgram>();
            if (clickAddCommandToSelectedProgram != null)
            {
                Destroy(clickAddCommandToSelectedProgram);
            }
        });
        
        
        _reorderableList.OnElementRemoved.AddListener((_) =>
        {
            if (_hasValidProgram != null)
            {
                if (_reorderableList.Content.childCount > 0)
                {
                    _hasValidProgram.Value = true;
                }
                else
                {
                    _hasValidProgram.Value = false;
                }
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
            clickDestroy = newCommandUI.gameObject.AddComponent<ClickDestroy>();
        }
        else
        {
            clickDestroy.onDestroy = null;
        }

        clickDestroy.onDestroy += () =>
        {
            if (_hasValidProgram != null)
            {
                if (_reorderableList.Content.childCount <= 1)
                {
                    _hasValidProgram.Value = false;
                }
                else
                {
                    _hasValidProgram.Value = true;
                }
            }
        };
        
        
        ClickAddCommandToSelectedProgram clickAddCommandToSelectedProgram =
            newCommandUI.GetComponent<ClickAddCommandToSelectedProgram>();
        if (clickAddCommandToSelectedProgram != null)
        {
            Destroy(clickAddCommandToSelectedProgram);
        }
        
        if (_hasValidProgram != null)
        {
            _hasValidProgram.Value = true;
        }
        
        _reorderableList.Refresh();
    }

    public void SetAsSelected()
    {
        Debug.Log(name + " selected");
        _titleBackgroundImage.color = _selectedColor.Value;
        
        foreach (Image backgroundImage in _backgroundImages)
        {
            backgroundImage.color =_selectedColor.Value;
        }
        
    }

    public void SetAsDeselected()
    {
        Debug.Log(name + " deselected");
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
        if (_hasValidProgram != null)
        {
            _hasValidProgram.Value = false;
        }
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
        if (_backgroundImages != null)
        {
            foreach (Image backgroundImage in _backgroundImages)
            {
                Destroy(backgroundImage.gameObject);
            }
            _backgroundImages.Clear();
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

        Vector2 newSizeDelta = new Vector2(83.7f * 4f, 83.7f * Mathf.CeilToInt((float)_backgroundImages.Count / 4f));

        _reordableListRectTransform.sizeDelta  = newSizeDelta;
    }
}
