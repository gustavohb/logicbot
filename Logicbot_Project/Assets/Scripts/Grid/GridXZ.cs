using System;
using TMPro;
using UnityEngine;

public class GridXZ<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> onGridObjectChanged;

    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }

    private int _width;
    private int _height;
    private float _cellSize;
    private Vector3 _originPosition;
    private TGridObject[,] _gridArray;

    public GridXZ(int width, int height, float cellSize, Vector3 originPosition, bool showDebug,
        Func<GridXZ<TGridObject>, int, int, TGridObject> createGridObject)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = originPosition;

        _gridArray = new TGridObject[width, height];

        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < _gridArray.GetLength(1); z++)
            {
                _gridArray[x, z] = createGridObject(this, x, z);
            }
        }

        if (showDebug)
        {
            TextMeshPro[,] debugTextArray = new TextMeshPro[width, height];

            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < _gridArray.GetLength(1); z++)
                {
                    debugTextArray[x, z] = UtilsClass.CreateWorldText(_gridArray[x, z]?.ToString(), null,
                        GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 3, Color.white,
                        TextAlignmentOptions.Top);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            onGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.z].text = _gridArray[eventArgs.x, eventArgs.z]?.ToString();
            };
        }
    }
    
    public TGridObject this[int i, int j]
    {
        get
        {
            return _gridArray[i, j];
        }
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }

    public float GetCellSize()
    {
        return _cellSize;
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * _cellSize + _originPosition; //TODO: Need to handle y position
    }
    
    public Vector3? GetWorldPosition(Vector2Int cellIndex)
    {
        if (cellIndex.x >= 0 && cellIndex.y >= 0 && cellIndex.x < _width && cellIndex.y < _height)
        {
            return new Vector3(cellIndex.x, 0, cellIndex.y) * _cellSize + _originPosition +
                   new Vector3(_cellSize, _cellSize, _cellSize) * .5f; // TODO: Need to handle y position
        }

        return null;
    }

    public void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        z = Mathf.FloorToInt((worldPosition - _originPosition).z / _cellSize);
    }

    public void SetGridObject(int x, int z, TGridObject value)
    {
        if (x >= 0 && z >= 0 && x < _width && z < _height)
        {
            _gridArray[x, z] = value;
            TriggerGridObjectChanged(x, z);
        }
    }

    public void TriggerGridObjectChanged(int x, int z)
    {
        onGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs {x = x, z = z});
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject obj)
    {
        GetXZ(worldPosition, out int x, out int z);
        SetGridObject(x, z, obj);
    }

    public TGridObject GetGridObject(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < _width && z < _height)
        {
            return _gridArray[x, z];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetGridObject(x, z);
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        return new Vector2Int(
            Mathf.Clamp(gridPosition.x, 0, _width - 1),
            Mathf.Clamp(gridPosition.y, 0, _height - 1)
        );
    }
}
