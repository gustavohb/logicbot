using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private int _gridWidth = 10;
    [SerializeField] private int _gridHeight = 10;
    [SerializeField] private float _cellSize = 1f;
    [SerializeField] private Transform _gridOriginTransform;

    [SerializeField] private bool _showDebugLines = true;
    
    [SerializeField] private PlacedTileRuntimeSet _placedTileRuntimeSet;
    
    private GridXZ<GridObject> _grid;
    
    protected override void Awake()
    {
        base.Awake();
        _grid = new GridXZ<GridObject>(_gridWidth, _gridHeight, _cellSize,
            _gridOriginTransform != null ? _gridOriginTransform.position : Vector3.zero,
            _showDebugLines,
            (GridXZ<GridObject> g, int x, int y) => new GridObject(g, x, y));
        
        _placedTileRuntimeSet.onRuntimeSetChanged += PlacedTileRuntimeSetOnRuntimeSetChanged;
    }

    private void PlacedTileRuntimeSetOnRuntimeSetChanged(object sender, BaseRuntimeSet<PlacedTile>.RuntimeSetEventArgs<PlacedTile> e)
    {
        if (!e.removed)
        {
            PlaceTileToTheGrid(e);
        }
        else
        {
            RemoveTileFromTheGrid(e);
        }
    }

    public PlacedTile GetPlacedTileAt(Vector3 worldPosition)
    {
        PlacedTile placedTile = null;
        GridObject gridObject = _grid.GetGridObject(worldPosition);
        if (gridObject != null)
        {
            placedTile = gridObject.GetPlacedTile();
        }

        return placedTile;
    }
    
    private void RemoveTileFromTheGrid(BaseRuntimeSet<PlacedTile>.RuntimeSetEventArgs<PlacedTile> e)
    {
        Vector3 placedTimeWorldPosition = e.obj.transform.position;
        GridObject gridObject = _grid.GetGridObject(placedTimeWorldPosition);
        if (gridObject != null && gridObject.IsOccupied())
        {
            _grid.GetGridObject(placedTimeWorldPosition).ClearPlacedObject();
            _grid.GetXZ(placedTimeWorldPosition, out int x, out int z);
        }
        else
        {
            Debug.LogError("Cannot remove " + e.obj.ToString() + " from the grid!");
        }
    }

    private void PlaceTileToTheGrid(BaseRuntimeSet<PlacedTile>.RuntimeSetEventArgs<PlacedTile> e)
    {
        Vector3 placedTimeWorldPosition = e.obj.transform.position;
        GridObject gridObject = _grid.GetGridObject(placedTimeWorldPosition);
        if (gridObject != null && !gridObject.IsOccupied())
        {
            _grid.GetGridObject(placedTimeWorldPosition).SetPlacedTile(e.obj);
            _grid.GetXZ(placedTimeWorldPosition, out int x, out int z);
        }
        else
        {
            Debug.LogError("Cannot add " + e.obj.ToString() + " to the grid, position is already occupied!");
        }
    }

    public Vector3 GetNextPosition(Vector3 currentPosition, PlayerController.Dir currentDirection)
    {   
        //TODO: Refactor
        _grid.GetXZ(currentPosition, out int x, out int z);
        Vector2Int nextTileIndex = new Vector2Int(x, z);
        switch (currentDirection)
        {
            case PlayerController.Dir.Right:
                nextTileIndex += new Vector2Int(1, 0);
                break;
            case PlayerController.Dir.Down:
                nextTileIndex += new Vector2Int(0, -1);
                break;
            case PlayerController.Dir.Left:
                nextTileIndex += new Vector2Int(-1, 0);
                break;
            case PlayerController.Dir.Up:
                nextTileIndex += new Vector2Int(0, 1);
                break;
        }

        Vector3 nextTileWorldPosition = _grid.GetWorldPosition(nextTileIndex.x, nextTileIndex.y);

        GridObject gridObject = _grid.GetGridObject(nextTileWorldPosition);
        
        if (gridObject != null && gridObject.IsOccupied())
        {
            Vector3? nextPosition = _grid.GetWorldPosition(nextTileIndex);

            if (nextPosition.HasValue)
            {
                float tileHeight = gridObject.GetPlacedTile().GetHeight();
                return nextPosition.Value + new Vector3(0, tileHeight, 0);
            }
        }
        
        
        return currentPosition;
    }
}
