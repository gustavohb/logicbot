using UnityEngine;

public class ProtagonistAudio : MonoBehaviour
{
    [SerializeField] private AudioCueSO jump, footstep, turnLightOn;

    
    [SerializeField] private AudioCueEventChannelSO _sfxEventChannel = default;
    [SerializeField] private AudioConfigurationSO _audioConfig = default;

    protected void PlayAudio(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace = default)
    {
        _sfxEventChannel.RaisePlayEvent(audioCue, audioConfiguration, positionInSpace);
    }
    
    public void PlayFootstep() => PlayAudio(footstep, _audioConfig, transform.position);
    public void PlayJump() => PlayAudio(jump, _audioConfig, transform.position);
    public void PlayTurnLightOn() => PlayAudio(turnLightOn, _audioConfig, transform.position);
}
