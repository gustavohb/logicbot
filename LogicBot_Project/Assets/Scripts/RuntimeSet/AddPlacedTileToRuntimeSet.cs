using UnityEngine;

[RequireComponent(typeof(PlacedTile))]
public class AddPlacedTileToRuntimeSet : MonoBehaviour
{

    [SerializeField] private PlacedTileRuntimeSet _runtimeSet = default;
    
    private PlacedTile _placedTile;
    private void Awake()
    {
        _placedTile = GetComponent<PlacedTile>();
    }

    private void OnEnable()
    {
        if (_runtimeSet != null && _placedTile != null)
        {
            Invoke(nameof(AddToRuntimeSet), 0.05f); // To fix bug
        }
    }

    private void AddToRuntimeSet()
    {
        _runtimeSet.AddToList(_placedTile);
    }

    private void OnDisable()
    {
        if (_runtimeSet != null && _placedTile != null)
        {
            _runtimeSet.RemoveFromList(_placedTile);
        }
    }
}
