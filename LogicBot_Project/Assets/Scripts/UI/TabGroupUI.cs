using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class TabGroupUI : MonoBehaviour
{
    [SerializeField] private List<TabButtonUI> _tabButtons;

    [SerializeField] private List<GameObject> _objectsToSwap = new List<GameObject>();

    [SerializeField] private Sprite _idleSprite;
    [SerializeField] private Sprite _hoverSprite;
    [SerializeField] private Sprite _activeSprite;

    [SerializeField] private Color _idleColor = Color.white;
    [SerializeField] private Color _hoverColor = Color.white;
    [SerializeField] private Color _activeColor = Color.white;
    
    private TabButtonUI _selectedTab;

    public void Subscribe(TabButtonUI buttonUI)
    {
        if (_tabButtons == null)
        {
            _tabButtons = new List<TabButtonUI>();
        }

        _tabButtons.Add(buttonUI);
    }

    public void SelectedTab(int tabIndex)
    {
        if (tabIndex > 0 && tabIndex < _tabButtons.Count)
        {
            OnTabSelected(_tabButtons[tabIndex]);
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
            buttonUI.targetGraphic.sprite = _hoverSprite;
            buttonUI.targetGraphic.color = _hoverColor;
        }
    }

    public void OnTabExit(TabButtonUI buttonUI)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButtonUI buttonUI)
    {
        if (_selectedTab != null)
        {
            _selectedTab.Deselect();
        }

        _selectedTab = buttonUI;
        _selectedTab.Select();
        ResetTabs();
        _selectedTab.targetGraphic.sprite = _activeSprite;
        _selectedTab.targetGraphic.color = _activeColor;
        int index = buttonUI.transform.GetSiblingIndex();
        for (int i = 0; i < _objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                _objectsToSwap[i].SetActive(true);
            }
            else
            {
                _objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs(bool resetAllTab = false)
    {
        foreach (TabButtonUI button in _tabButtons)
        {
            if (_selectedTab != null && button == _selectedTab && !resetAllTab)
            {
                continue;
            }

            button.targetGraphic.sprite = _idleSprite;
            button.targetGraphic.color = _idleColor;
        }
    }
}


