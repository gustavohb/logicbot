using System;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioCueEventChannelSO _playMusicOn = default;
    [SerializeField] private AudioConfigurationSO _audioConfig = default;
    [SerializeField] private AudioCueSO _gameMusic = default;
    
    private void Start()
    {
        PlayMusic();
    }

    private void PlayMusic()
    {
        _playMusicOn.RaisePlayEvent(_gameMusic, _audioConfig);
    }
}
