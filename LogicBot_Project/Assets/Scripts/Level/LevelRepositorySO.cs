using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Level/Repository")]
public class LevelRepositorySO : ScriptableObject
{
    public List<LevelDataSO> levelList = new List<LevelDataSO>();
}
