using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private TabGroup _tabGroup;

    public UnityEvent onSelected;
    public UnityEvent onDeselected;

    public Image targetGraphic;

    private void Start()
    {
        targetGraphic = GetComponent<Image>();
        if (_tabGroup != null)
        {
            _tabGroup.Subscribe(this);
        }
    }

    public void SetTabGroup(TabGroup tabGroup)
    {
        _tabGroup = tabGroup;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tabGroup.OnTabEnter(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _tabGroup.OnTabSelected(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tabGroup.OnTabExit(this);
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
