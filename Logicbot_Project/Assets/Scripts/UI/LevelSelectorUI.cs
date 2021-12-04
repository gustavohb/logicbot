using ScriptableObjectArchitecture;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TabGroupUI))]
public class LevelSelectorUI : MonoBehaviour
{
    [SerializeField] private IntVariable _totalLevels;
    [SerializeField] private IntGameEvent _loadLevelEvent;
    [SerializeField] private BoolVariable _isLoadingLevel;
    [SerializeField] private IntVariable _currentLevelIndex;
    
    private TabGroupUI _tabGroupUI;

    private void Awake()
    {
        _tabGroupUI = GetComponent<TabGroupUI>();
    }

    private void Start()
    {
        InitializeUI();
        
        this.Wait(1f, () =>
        {
            _currentLevelIndex.Value = 0;
        });
    }

    private void InitializeUI()
    {
        Transform levelButtonTemplate = transform.Find("levelButtonTemplate");
        levelButtonTemplate.gameObject.SetActive(false);
        for (int i = 0; i < _totalLevels.Value; i++)
        {
            Transform newLevelButtonTransform = Instantiate(levelButtonTemplate, transform);
            newLevelButtonTransform.gameObject.SetActive(true);
            
            TextMeshProUGUI levelTextNumber =
                newLevelButtonTransform.transform.Find("number").GetComponent<TextMeshProUGUI>();
            levelTextNumber.SetText((i + 1).ToString());
            
            TabButtonUI tabButtonUI = newLevelButtonTransform.GetComponent<TabButtonUI>();
            tabButtonUI.SetTabGroup(_tabGroupUI);
            int levelIndex = i;
            tabButtonUI.onSelected.AddListener(() =>
            {
                if (_isLoadingLevel.Value)
                {
                    return;
                }
                _loadLevelEvent.Raise(levelIndex);
            });            
            _tabGroupUI.Subscribe(tabButtonUI);
        }
        _tabGroupUI.ResetTabs();
    }
    
}
