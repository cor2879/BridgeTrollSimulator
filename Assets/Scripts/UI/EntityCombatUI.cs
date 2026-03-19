using UnityEngine;
using TMPro;

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
        private GameObject statsContainer;
        [SerializeField]
        private Canvas combatUIPanel;
        
        private EntityController entity;

        public Canvas CombatUIPanel => combatUIPanel;
        public Transform Transform => CombatUIPanel.transform;

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

        public void Refresh()
        {
            hpText.text = $"HP: {entity.CurrentHealth}";
            staminaText.text = $"ST: {entity.CurrentStamina}/{entity.MaxStamina}";
        }

        public void SetActive(bool active)
        {
            statsContainer.gameObject.SetActive(active);
        }

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
    }
}