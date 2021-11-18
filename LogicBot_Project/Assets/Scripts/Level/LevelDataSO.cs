using UnityEngine;

[CreateAssetMenu(menuName = "Game/LevelData")]
public class LevelDataSO : ScriptableObject
{
    public LevelSolutionSO solution;
    public LevelTiles levelTiles;
    public PlayerController.Dir playerStartDir;
}
