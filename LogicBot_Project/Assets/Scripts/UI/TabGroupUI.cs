using System.Collections.Generic;
using JetBrains.Annotations;
using ScriptableObjectArchitecture;
using UnityEngine;

public class TabGroupUI : MonoBehaviour
{
    [SerializeField] private List<TabButtonUI> _tabButtons;

    [SerializeField] private List<GameObject> _objectsToSwap = new List<GameObject>();

    [SerializeField] private Color _idleColor = Color.white;
    [SerializeField] private Color _hoverColor = Color.white;
    [SerializeField] private Color _activeColor = Color.white;
    
    [SerializeField] private GameObject _defaultObject;

    [SerializeField] private BoolVariable _isDisable;

    [SerializeField] private IntVariable _currentSelectedTabIndex;
    
    private TabButtonUI _selectedTab;

    private void OnEnable()
    {
        if (_currentSelectedTabIndex != null)
        {
            _currentSelectedTabIndex.AddListener(SelectTab);    
        }
    }

    public void Subscribe(TabButtonUI buttonUI)
    {
        if (_tabButtons == null)
        {
            _tabButtons = new List<TabButtonUI>();
        }

        _tabButtons.Add(buttonUI);
    }

    public void SelectTab(int tabIndex)
    {
        if (tabIndex >= 0 && tabIndex < _tabButtons.Count)
        {
            if (_isDisable != null && _isDisable.Value)
            {
                return;
            }
            else
            {
                OnTabSelected(_tabButtons[tabIndex]);
            }
        }
    }
    

    [CanBeNull]
    public TabButtonUI GetSelectedTab()
    {
        return _selectedTab;
    }

    public void OnTabEnter(TabButtonUI buttonUI)
    {
        ResetTabs();
        if (_selectedTab == null || buttonUI != _selectedTab)
        {
            if (buttonUI.hoverSprite != null)
            {
                buttonUI.targetGraphic.sprite = buttonUI.hoverSprite;    
            }
            buttonUI.targetGraphic.color = _hoverColor;
        }
    }

    public void OnTabExit(TabButtonUI buttonUI)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButtonUI buttonUI)
    {
        if (_isDisable != null && _isDisable.Value)
        {
            return;
        }
        
        if (_defaultObject != null && buttonUI == _selectedTab)
        {
            _selectedTab.targetGraphic.sprite = _selectedTab.idleSprite;
            _selectedTab.Deselect();
            DisableAllObjectsToSwap();
            _defaultObject.SetActive(true);
            _selectedTab = null;
            return;
        }
        
        if (_selectedTab != null)
        {
            _selectedTab.Deselect();
        }

        _selectedTab = buttonUI;
        _selectedTab.Select();
        ResetTabs();
        if (_selectedTab.activeSprite != null)
        {
            _selectedTab.targetGraphic.sprite = _selectedTab.activeSprite;
        }
        
        _selectedTab.targetGraphic.color = _activeColor;
        int index = buttonUI.transform.GetSiblingIndex();

        if (_objectsToSwap != null && _objectsToSwap.Count > 0 && index < _objectsToSwap.Count)
        {
            DisableAllObjectsToSwap();
            _objectsToSwap[index].SetActive(true);    
        }
    }

    private void DisableAllObjectsToSwap()
    {
        for (int i = 0; i < _objectsToSwap.Count; i++)
        {
            _objectsToSwap[i].SetActive(false);
        }
    }

    public void UpdateUI()
    {
        ResetTabs();
        
    }
    
    public void ResetTabs(bool resetAllTab = false)
    {
        foreach (TabButtonUI button in _tabButtons)
        {
            if (_selectedTab != null && button == _selectedTab && !resetAllTab)
            {
                continue;
            }
            if (button.idleSprite != null)
            {
                button.targetGraphic.sprite = button.idleSprite;    
            }
            button.targetGraphic.color = _idleColor;
        }
    }
    
    private void OnDisable()
    {
        if (_currentSelectedTabIndex != null)
        {
            _currentSelectedTabIndex.RemoveListener(SelectTab);
        }
    }
}


