using UnityEngine;

namespace ScriptableObjectArchitecture
{
    [System.Serializable]
    [CreateAssetMenu(
        fileName = "ColorVariableGameEvent.asset",
        menuName = SOArchitecture_Utility.GAME_EVENT + "ColorVariable",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_EVENTS + 1)]
    public class ColorVariableGameEvent : GameEventBase<ColorVariable>
    {
    } 
}