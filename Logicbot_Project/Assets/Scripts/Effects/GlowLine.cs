using UnityEngine;
using Random = UnityEngine.Random;

public class GlowLine : MonoBehaviour
{
    [SerializeField] private Texture[] _textures;

    [SerializeField] private LineRenderer _lineRenderer;

    [SerializeField] private float _frameDuration = 0.1f;

    private float _timer;

    private void Start()
    {
        _timer = _frameDuration;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            _lineRenderer.material.SetTexture("_MainTex", _textures[Random.Range(0, _textures.Length)]);

            _timer = _frameDuration;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        //Update();
    }
}
