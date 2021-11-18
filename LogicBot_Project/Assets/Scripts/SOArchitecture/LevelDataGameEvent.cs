using UnityEngine;

namespace ScriptableObjectArchitecture
{
    [System.Serializable]
    [CreateAssetMenu(
        fileName = "LevelDataGameEvent.asset",
        menuName = SOArchitecture_Utility.GAME_EVENT + "SOs/LevelData",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_EVENTS + 20)]
    public sealed class LevelDataGameEvent : GameEventBase<LevelDataSO>
    {
    } 
}