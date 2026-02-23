using System.Collections;
using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Feedback
{
    public class CombatFeedbackSystem : MonoBehaviour
    {
        [SerializeField]
        private ShakeBehaviour cameraShake;

        private void OnEnable()
        {
            GameEventBus.Subscribe<DamageTakenEvent>(OnDamageTaken);
            GameEventBus.Subscribe<EntityDiedEvent>(OnEntityDied);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<DamageTakenEvent>(OnDamageTaken);
            GameEventBus.Unsubscribe<EntityDiedEvent>(OnEntityDied);
        }

        private void OnDamageTaken(DamageTakenEvent evt)
        {
            var isCrit = evt.IsCrit;
            var duration = isCrit ? 0.6f : 0.375f;
            var magnitude = isCrit ? 0.4f : 0.2f;

            StartCoroutine(DoHitStop(duration * 0.5f));

            if (cameraShake != null)
            {
                Debug.Log("camera shake");
                cameraShake.StartShake(duration, magnitude, 3f);
            }
        }

        private void OnEntityDied(EntityDiedEvent evt)
        {
            StartCoroutine(DoHitStop(0.2f));

            cameraShake.StartShake(0.2f, 0.5f);
        }

        private IEnumerator DoHitStop(float duration)
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1f;
        }
    }
}