using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TabGroupUI))]
public class MusicSelectorUI : MonoBehaviour
{
    [SerializeField] private List<AudioCueSO> _musicList = new List<AudioCueSO>();

    [SerializeField] private AudioCueEventChannelSO _playMusicOn = default;
    [SerializeField] private AudioConfigurationSO _audioConfig = default;
    
    private TabGroupUI _tabGroupUI;

    private void Awake()
    {
        _tabGroupUI = GetComponent<TabGroupUI>();
    }

    private void Start()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        Transform musicButtonTemplate = transform.Find("musicButtonTemplate");
        musicButtonTemplate.gameObject.SetActive(false);
        for (int i = 0; i < _musicList.Count; i++)
        {
            Transform newMusicButtonTransform = Instantiate(musicButtonTemplate, transform);
            newMusicButtonTransform.gameObject.SetActive(true);
            
            TextMeshProUGUI musicTextNumber =
                newMusicButtonTransform.transform.Find("number").GetComponent<TextMeshProUGUI>();
            musicTextNumber.SetText((i + 1).ToString());
            
            TabButtonUI tabButtonUI = newMusicButtonTransform.GetComponent<TabButtonUI>();
            tabButtonUI.SetTabGroup(_tabGroupUI);
            int musicNum = i;
            tabButtonUI.onSelected.AddListener(() =>
            {
                _playMusicOn.RaisePlayEvent(_musicList[musicNum], _audioConfig);  
            });            
            _tabGroupUI.Subscribe(tabButtonUI);
        }
        _tabGroupUI.ResetTabs();
        _tabGroupUI.SelectedTab(5);
    }
}
