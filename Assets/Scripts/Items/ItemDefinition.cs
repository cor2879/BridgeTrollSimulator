using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Items
{
    [CreateAssetMenu(menuName = "Items/Item")]
    public class ItemDefinition : ScriptableObject
    {
        public string ItemName;
        public string Description;
        public int Cost;
    }
}