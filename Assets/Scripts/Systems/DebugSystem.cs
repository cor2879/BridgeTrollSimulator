using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class DebugSystem : MonoBehaviour
    {
        private void OnEnable()
        {
            GameEventBus.Subscribe<CombatStartedEvent>(OnCombatStarted);
            GameEventBus.Subscribe<GoldDeductedEvent>(OnGoldDeducted);
            GameEventBus.Subscribe<GameStateChangedEvent>(OnGameStateChanged);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<CombatStartedEvent>(OnCombatStarted);
            GameEventBus.Unsubscribe<GoldDeductedEvent>(OnGoldDeducted);
            GameEventBus.Unsubscribe<GameStateChangedEvent>(OnGameStateChanged);
        }

        private void OnCombatStarted(CombatStartedEvent evt)
        {
            Debug.Log(evt.ToString());
        }

        private void OnGoldDeducted(GoldDeductedEvent evt)
        {
            Debug.Log(evt.ToString());
        }

        private void OnGameStateChanged(GameStateChangedEvent evt)
        {
            Debug.Log(evt.ToString());
        }
    }
}