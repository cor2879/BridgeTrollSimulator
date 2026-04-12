using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    [RequireComponent(typeof(EntityController))]
    public class EntityCombatUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text hpText;
        [SerializeField]
        private TMP_Text staminaText;
        [SerializeField]
        private Transform statusEffectContainer;
        [SerializeField]
        private GameObject statsContainer;
        [SerializeField]
        private Canvas combatUIPanel;
        [Header("Prefabs")]
        [SerializeField]
        private StatusEffectIconUI statusEffectIconPrefab;
        
        private EntityController entity;
        private Dictionary<string, StatusEffectIconUI> activeEffects = new();

        public Canvas CombatUIPanel => combatUIPanel;
        public Transform Transform => CombatUIPanel.transform;

#region Unity Hooks
        private void Update()
        {
            Refresh();
        }
        
        private void Awake()
        {
            entity = GetComponent<EntityController>();
            Refresh();
        }

        private void OnEnable()
        {
            GameEventBus.Subscribe<CombatConfirmedEvent>(OnCombatConfirmed);
            GameEventBus.Subscribe<CombatEndedEvent>(OnCombatEnded);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<CombatConfirmedEvent>(OnCombatConfirmed);
            GameEventBus.Unsubscribe<CombatEndedEvent>(OnCombatEnded);
        }
#endregion

#region Public Interface
        public void Refresh()
        {
            hpText.text = $"HP: {entity.CurrentHealth}";
            staminaText.text = $"ST: {entity.CurrentStamina}/{entity.MaxStamina}";
        }

        public void SetActive(bool active)
        {
            statsContainer.gameObject.SetActive(active);
        }
#endregion

#region Event Handlers
        public void OnCombatConfirmed(CombatConfirmedEvent evt)
        {
            if ((EntityController)evt.Initiator != entity && (EntityController)evt.Target != entity)
            {
                return;
            }

            SetActive(true);
        }

        public void OnCombatEnded(CombatEndedEvent evt)
        {
            if ((EntityController)evt.Initiator != entity && (EntityController)evt.Target != entity)
            {
                return;
            }
            
            SetActive(false);
        }

        public void OnEffectApplied(StatusEffectAppliedEvent evt)
        {
            var effect = evt.Effect;

            if (activeEffects.TryGetValue(effect.EffectTypeName, out var existingIcon))
            {
                existingIcon.SetDuration(effect.Duration);
                existingIcon.PlayTickFeedback(effect);
                return;
            }

            var icon = Instantiate(statusEffectIconPrefab, statusEffectContainer);

            icon.Initialize(
                effect.EffectTypeName,
                effect.Icon,
                effect.IconColor,
                effect.Duration);

            activeEffects.Add(effect.EffectTypeName, icon);
        }

        public void OnEffectTicked(StatusEffectTickEvent evt)
        {
            var effect = evt.Effect;

            if (!activeEffects.TryGetValue(effect.EffectTypeName, out var icon))
            {
                return;
            }

            icon.DecrementDuration();

            icon.PlayTickFeedback(evt.Effect);
        }

        public void OnEffectExpired(StatusEffectExpiredEvent evt)
        {
            var effect = evt.Effect;

            if (!activeEffects.TryGetValue(effect.EffectTypeName, out var icon))
            {
                return;
            }

            activeEffects.Remove(effect.EffectTypeName);
            icon.ExpireAndDestroy();
        }
#endregion
    }
}