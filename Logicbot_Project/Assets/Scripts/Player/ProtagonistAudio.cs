using UnityEngine;

public class ProtagonistAudio : MonoBehaviour
{
    [SerializeField] private AudioCueSO _jump, _footstep, _turnLightOn, _changeColor, _teleport;

    
    [SerializeField] private AudioCueEventChannelSO _sfxEventChannel = default;
    [SerializeField] private AudioConfigurationSO _audioConfig = default;

    protected void PlayAudio(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace = default)
    {
        _sfxEventChannel.RaisePlayEvent(audioCue, audioConfiguration, positionInSpace);
    }
    
    public void PlayFootstep() => PlayAudio(_footstep, _audioConfig, transform.position);
    public void PlayJump() => PlayAudio(_jump, _audioConfig, transform.position);
    public void PlayTurnLightOn() => PlayAudio(_turnLightOn, _audioConfig, transform.position);
    public void PlayChangeColor() => PlayAudio(_changeColor, _audioConfig, transform.position);

    public void PlayTeleport() => PlayAudio(_teleport, _audioConfig, transform.position);
}
