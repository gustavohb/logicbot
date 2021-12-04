using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ArcLineRenderer : MonoBehaviour
{
    [SerializeField] private float _velocity;
    [SerializeField] private float _angle;
    [SerializeField] private int _resolution;

    float g; //force of gravity on the y axis
    float radianAngle;
    
    LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics2D.gravity.y);
    }

    private void OnValidate()
    {
        if(lr!=null && Application.isPlaying){
            RenderArc();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        RenderArc();
    }

    //initialization
    void RenderArc()
    {
        // obsolete: lr.SetVertexCount(resolution + 1);
        lr.positionCount = _resolution + 1;
        lr.SetPositions(CalculateArcArray());
    }
    //Create an array of Vector 3 positions for the arc
    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[_resolution + 1];

        radianAngle = Mathf.Deg2Rad * _angle;
        float maxDistance = (_velocity * _velocity * Mathf.Sin(2 * radianAngle)) / g;

        for (int i = 0; i <= _resolution; i++)
        {
            float t = (float)i / (float)_resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }

        return arcArray;
    }

    Vector3 CalculateArcPoint(float t, float maxDistance)
    {
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) - ((g * x * x) / (2 * _velocity * _velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        return new Vector3(x, y);
    }

}