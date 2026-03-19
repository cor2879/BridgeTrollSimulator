using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Feedback.Events;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Effects
{
    [RequireComponent(typeof(EntityController))]
    public class FlashEffect : MonoBehaviour
    {
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
            GameEventBus.Subscribe<FlashEvent>(OnFlash);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<FlashEvent>(OnFlash);
        }

        private void OnFlash(FlashEvent evt)
        {
            if (evt.Target as EntityController != entity)
                return;

            StartFlash(evt.Color, evt.Duration);
        }

        private void StartFlash(Color color, float duration = 0.15f)
        {
            if (!isFlashing)
            {
                originalColor = spriteRenderer.color;
                isFlashing = true;
            }

            spriteRenderer.color = color;
            flashEndTime = Time.time + duration;
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