
public class GridObject
{
    private GridXZ<GridObject> _grid;
    private int _x;
    private int _z;
    private PlacedTile _placedTile;

    public GridObject(GridXZ<GridObject> grid, int x, int z)
    {
        _grid = grid;
        _x = x;
        _z = z;
        _placedTile = null;
    }

    public override string ToString()
    {
        return _x + ", " + _z + "\n" + _placedTile;
    }

    public void SetPlacedTile(PlacedTile placedTile)
    {
        _placedTile = placedTile;
        _grid.TriggerGridObjectChanged(_x, _z);
    }

    public bool IsOccupied()
    {
        return _placedTile != null;
    }

    public void ClearPlacedObject()
    {
        _placedTile = null;
        _grid.TriggerGridObjectChanged(_x, _z);
    }
    
    

    public PlacedTile GetPlacedTile()
    {
        return _placedTile;
    }
}
