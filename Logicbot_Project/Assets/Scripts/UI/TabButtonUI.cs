using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private TabGroupUI _tabGroupUI;

    public Sprite idleSprite;
    public Sprite hoverSprite;
    public Sprite activeSprite;

    public UnityEvent onSelected;
    public UnityEvent onDeselected;

    public Image targetGraphic;

    private void Start()
    {
        targetGraphic = GetComponent<Image>();
        if (_tabGroupUI != null)
        {
            _tabGroupUI.Subscribe(this);
        }
    }

    public void SetTabGroup(TabGroupUI tabGroupUI)
    {
        _tabGroupUI = tabGroupUI;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tabGroupUI.OnTabEnter(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _tabGroupUI.OnTabSelected(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tabGroupUI.OnTabExit(this);
    }

    public void Select()
    {
        if (onSelected != null)
        {
            onSelected.Invoke();
        }
    }

    public void Deselect()
    {
        if (onDeselected != null)
        {
            onDeselected.Invoke();
        }
    }
}
