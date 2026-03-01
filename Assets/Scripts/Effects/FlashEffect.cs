using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Effects
{
    [RequireComponent(typeof(EntityController))]
    public class FlashEffect : MonoBehaviour
    {
        [SerializeField] private float flashDuration = 0.15f;
        [SerializeField] private Color flashColor = new Color(1f, 0.3f, 0.3f);

        private EntityController entity;
        private SpriteRenderer spriteRenderer;

        private float flashEndTime = -1f;
        private Color originalColor;
        [SerializeField, ReadOnly]
        private bool isFlashing = false;

        private void Awake()
        {
            entity = GetComponent<EntityController>();
            spriteRenderer = entity.SpriteRenderer;
        }

        private void OnEnable()
        {
            GameEventBus.Subscribe<DamageTakenEvent>(OnDamageTaken);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<DamageTakenEvent>(OnDamageTaken);
        }

        private void OnDamageTaken(DamageTakenEvent evt)
        {
            if (evt.Target != entity)
                return;

            StartFlash();
        }

        private void StartFlash()
        {
            if (!isFlashing)
            {
                originalColor = spriteRenderer.color;
                isFlashing = true;
            }

            spriteRenderer.color = flashColor;
            flashEndTime = Time.time + flashDuration;
        }

        private void Update()
        {
            if (!isFlashing)
                return;

            if (Time.time >= flashEndTime)
            {
                spriteRenderer.color = originalColor;
                isFlashing = false;
            }
        }
    }
}