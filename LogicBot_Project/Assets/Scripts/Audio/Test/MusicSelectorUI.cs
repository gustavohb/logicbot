using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TabGroup))]
public class MusicSelectorUI : MonoBehaviour
{
    [SerializeField] private List<AudioCueSO> _musicList = new List<AudioCueSO>();

    [SerializeField] private AudioCueEventChannelSO _playMusicOn = default;
    [SerializeField] private AudioConfigurationSO _audioConfig = default;
    
    private TabGroup _tabGroup;

    private void Awake()
    {
        _tabGroup = GetComponent<TabGroup>();
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
            
            TabButton tabButton = newMusicButtonTransform.GetComponent<TabButton>();
            
            int musicNum = i;
            tabButton.onSelected.AddListener(() =>
            {
                _playMusicOn.RaisePlayEvent(_musicList[musicNum], _audioConfig);  
            });            
            _tabGroup.Subscribe(tabButton);
        }
        _tabGroup.ResetTabs();
        _tabGroup.SelectedTab(5);
    }
}
