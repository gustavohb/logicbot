using UnityEngine;

public class ProtagonistAudio : MonoBehaviour
{
    [SerializeField] private AudioCueSO liftoff, land, footstep, turnLightOn;

    
    [SerializeField] private AudioCueEventChannelSO _sfxEventChannel = default;
    [SerializeField] private AudioConfigurationSO _audioConfig = default;

    protected void PlayAudio(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace = default)
    {
        _sfxEventChannel.RaisePlayEvent(audioCue, audioConfiguration, positionInSpace);
    }
    
    public void PlayFootstep() => PlayAudio(footstep, _audioConfig, transform.position);
    public void PlayJumpLiftoff() => PlayAudio(liftoff, _audioConfig, transform.position);
    public void PlayJumpLand() => PlayAudio(land, _audioConfig, transform.position);
    public void PlayTurnLightOn() => PlayAudio(turnLightOn, _audioConfig, transform.position);
}
