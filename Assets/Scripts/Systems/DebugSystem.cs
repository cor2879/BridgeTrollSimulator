using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class DebugSystem : MonoBehaviour
    {
        private void OnEnable()
        {
            GameEventBus.Subscribe<CombatStartedEvent>(OnCombatStarted);
            GameEventBus.Subscribe<GoldDeductedEvent>(OnGoldDeducted);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<CombatStartedEvent>(OnCombatStarted);
            GameEventBus.Unsubscribe<GoldDeductedEvent>(OnGoldDeducted);
        }

        private void OnCombatStarted(CombatStartedEvent evt)
        {
            Debug.Log(evt.ToString());
        }

        private void OnGoldDeducted(GoldDeductedEvent evt)
        {
            Debug.Log(evt.ToString());
        }
    }
}