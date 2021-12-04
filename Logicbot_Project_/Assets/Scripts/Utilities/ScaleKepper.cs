using System;
using UnityEngine;

public class ScaleKepper : MonoBehaviour
{
    private Vector3 _originalScale;
    private Transform _transform;
    
    private void Awake()
    {
        _transform = transform;
        _originalScale = _transform.localScale;
    }

    private void Update()
    {
        SetScale();
    }

    public void SetScale()
    {
        _transform.localScale = _originalScale;
    }
}
