using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class AddPlayerControllerToRuntimeSet : MonoBehaviour
{

    [SerializeField] private PlayerControllerRuntimeSet _runtimeSet = default;


    private PlayerController _playerController;
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        if (_runtimeSet != null && _playerController != null)
        {
            Invoke(nameof(AddToRuntimeSet), 0.05f); // To fix bug
        }
    }

    private void AddToRuntimeSet()
    {
        _runtimeSet.AddToList(_playerController);
    }

    private void OnDisable()
    {
        if (_runtimeSet != null && _playerController != null)
        {
            _runtimeSet.RemoveFromList(_playerController);
        }
    }
}