using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(LineRenderer) )]
public class CurvedLineRenderer : MonoBehaviour 
{
	[Header("Settings")]
	[SerializeField] private float _lineSegmentSize = 0.15f;
	[SerializeField] private float _lineWidth = 0.1f;
	[SerializeField] private bool _testMode = false;

	[Header("References")]
	[SerializeField] private LineRenderer _lineRenderer;
	[SerializeField] private LineRenderer _glowLineRenderer;
	
	[Header("Gizmos")]
	[SerializeField] private bool _showGizmos = true;
	[SerializeField] private float _gizmoSize = 0.1f;
	[SerializeField] private Color _gizmoColor = new Color(1,0,0,0.5f);
	
	[SerializeField] private CurvedLinePoint _startPoint;
	[SerializeField] private CurvedLinePoint _middlePoint;
	[SerializeField] private CurvedLinePoint _endPoint;

	private Vector3 _startLinePosition;
	private Vector3 _middleLinePosition;
	private Vector3 _endLinePosition;
	
	private Vector3 _startLinePositionOld;
	private Vector3 _middleLinePositionOld;
	private Vector3 _endLinePositionOld;

	private void Awake()
	{
		_lineRenderer = GetComponent<LineRenderer>();
	}

	public void Update() 
	{
		GetPoints();
		SetPointsToLine();
	}

	private void GetPoints()
	{
		if (_startPoint != null)
		{
			_startLinePosition = _startPoint.transform.position;
		}
		
		if (_middlePoint != null)
		{
			_middleLinePosition = _middlePoint.transform.position;
		}
		
		if (_endPoint != null)
		{
			_endLinePosition = _endPoint.transform.position;
		}
	}

	public void SetStartPoint(Vector3 position, Transform parent)
	{
		GameObject newGameObject = new GameObject("StartPoint");
		newGameObject.transform.position = position;
		newGameObject.transform.parent = parent;
		_startPoint = newGameObject.AddComponent<CurvedLinePoint>();
		_startPoint.showGizmo = _showGizmos;
		_startPoint.gizmoSize = _gizmoSize;
		_startPoint.gizmoColor = _gizmoColor;
	}
	
	public void SetMiddlePointParent(Transform parent)
	{
		GameObject newGameObject = new GameObject("MiddlePoint");
		newGameObject.transform.position = CalculateMiddlePoint();
		newGameObject.transform.parent = parent;
		_middlePoint = newGameObject.AddComponent<CurvedLinePoint>();
		_middlePoint.showGizmo = _showGizmos;
		_middlePoint.gizmoSize = _gizmoSize;
		_middlePoint.gizmoColor = _gizmoColor;
	}
	
	private Vector3 CalculateMiddlePoint()
	{
		return (_startPoint.transform.position + _endPoint.transform.position) / 2f + new Vector3(0, 3, 0); 
	}
	
	public void SetEndPoint(Vector3 position, Transform parent)
	{
		GameObject newGameObject = new GameObject("EndPoint");
		newGameObject.transform.position = position;
		newGameObject.transform.parent = parent;
		_endPoint = newGameObject.AddComponent<CurvedLinePoint>();
		_endPoint.showGizmo = _showGizmos;
		_endPoint.gizmoSize = _gizmoSize;
		_endPoint.gizmoColor = _gizmoColor;
	}

	private void SetPointsToLine()
	{
		//check if line points have moved
		bool moved = false;

		if (_startPoint == null || _middlePoint == null || _endPoint == null)
		{
			return;
		}
		
		if (_startLinePosition != _startLinePositionOld)
		{
			_startLinePositionOld = _startLinePosition;
			moved = true;
		}
		
		if (_middleLinePosition != _middleLinePositionOld)
		{
			_middleLinePositionOld = _middleLinePosition;
			moved = true;
		}
		
		if (_endLinePosition != _endLinePositionOld)
		{
			_endLinePositionOld = _endLinePosition;
			moved = true;
		}
		

		//update if moved
		if(moved || _testMode)
		{
			List<Vector3> linePositions = new List<Vector3>()
			{
				_startLinePosition,
				_middleLinePosition,
				_endLinePosition,
			};
			
			//get smoothed values
			Vector3[] smoothedPoints = LineSmoother.SmoothLine(linePositions, _lineSegmentSize);

			//set line settings
			_lineRenderer.positionCount = smoothedPoints.Length;
			_lineRenderer.SetPositions(smoothedPoints);

			if (_glowLineRenderer != null)
			{
				_glowLineRenderer.positionCount = smoothedPoints.Length;
				_glowLineRenderer.SetPositions(smoothedPoints);
			}

			_lineRenderer.startWidth = _lineWidth;
			_lineRenderer.endWidth = _lineWidth;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Update();
	}
}
