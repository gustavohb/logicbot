using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class DissolveObject : MonoBehaviour
{
    [SerializeField] private FloatVariable _duration;

    [SerializeField] private GameEvent _dissolvePlayerGameEvent;
    [SerializeField] private GameEvent _condensePlayerGameEvent;
    
    private const float _minDissolveValue = 0f;
    private const float _maxDissolveValue = 1f;

    private Material _material;

    private float _currentDissolveValue;

    private void OnEnable()
    {
        _dissolvePlayerGameEvent.AddListener(Dissolve);
        _condensePlayerGameEvent.AddListener(Condense);
    }

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void Start()
    {
        _currentDissolveValue = _minDissolveValue;
    }

    private void Update()
    {
        SetDissolveValue(_currentDissolveValue);
    }

    private void Dissolve()
    {
        float to = _maxDissolveValue;
        DOTween.To(() => _currentDissolveValue, x => _currentDissolveValue = x, to, _duration.Value);
    }

    private void Condense()
    {
        float to = _minDissolveValue;
        DOTween.To(() => _currentDissolveValue, x => _currentDissolveValue = x, to, _duration.Value);
    }

    private void SetDissolveValue(float dissolveValue)
    {
        _material.SetFloat("_DissolveValue", dissolveValue);
    }

    private void OnDisable()
    {
        _dissolvePlayerGameEvent.RemoveListener(Dissolve);
        _condensePlayerGameEvent.RemoveListener(Condense);
    }
}