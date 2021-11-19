using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [SerializeField] private List<TabButton> _tabButtons;

    [SerializeField] private List<GameObject> _objectsToSwap = new List<GameObject>();

    [SerializeField] private Sprite _idleSprite;
    [SerializeField] private Sprite _hoverSprite;
    [SerializeField] private Sprite _activeSprite;

    [SerializeField] private Color _idleColor = Color.white;
    [SerializeField] private Color _hoverColor = Color.white;
    [SerializeField] private Color _activeColor = Color.white;
    
    private TabButton _selectedTab;

    public void Subscribe(TabButton button)
    {
        if (_tabButtons == null)
        {
            _tabButtons = new List<TabButton>();
        }

        _tabButtons.Add(button);
    }


    [CanBeNull]
    public TabButton GetSelectedTab()
    {
        return _selectedTab;
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (_selectedTab == null || button != _selectedTab)
        {
            button.targetGraphic.sprite = _hoverSprite;
            button.targetGraphic.color = _hoverColor;
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        if (_selectedTab != null)
        {
            _selectedTab.Deselect();
        }

        _selectedTab = button;
        _selectedTab.Select();
        ResetTabs();
        _selectedTab.targetGraphic.sprite = _activeSprite;
        _selectedTab.targetGraphic.color = _activeColor;
        int index = button.transform.GetSiblingIndex();
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
        foreach (TabButton button in _tabButtons)
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


