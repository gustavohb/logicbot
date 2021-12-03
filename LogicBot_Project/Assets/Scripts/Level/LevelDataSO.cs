using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Game/Level/LevelData")]
public class LevelDataSO : ScriptableObject
{
    public LevelSolutionSO solution;
    public  AssetReference levelTiles;
    public PlayerController.Dir playerStartDir;
}
