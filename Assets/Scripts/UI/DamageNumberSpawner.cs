using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Feedback.Events;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class DamageNumberSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject damageNumberPrefab;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Vector3 worldOffset = new Vector3(0, 1.5f, 0);
        [SerializeField] private UnityEngine.Camera worldCamera;

        private void OnEnable()
        {
            GameEventBus.Subscribe<FloatingTextEvent>(OnFloatingText);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<FloatingTextEvent>(OnFloatingText);
        }

        private void OnFloatingText(FloatingTextEvent evt)
        {
            var combatUI = ((EntityController)evt.Target).CombatUI;

            if (combatUI == null)
            {
                return;
            }

            Spawn(combatUI.Transform, evt.Amount, evt.IsCrit, evt.Color);
        }

        private void Spawn(Transform parent, int amount, bool isCrit, Color color)
        {
            GameObject obj = Instantiate(damageNumberPrefab, parent);
            obj.transform.localPosition = Vector3.zero;

            obj.GetComponent<DamageNumber>().Initialize(amount, isCrit, color);
        }
    }
}