using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class SocialDuelUI : MonoBehaviour, IEventSource
    {
        [Header("Ability Buttons")]
        [SerializeField] private Transform abilityButtonContainer;
        [SerializeField] private Button abilityButtonPrefab;

        [Header("Resolve Bars")]
        [SerializeField] private ResolveBarUI playerBar;
        [SerializeField] private ResolveBarUI npcBar;

        private SocialDuelContext context;
        private readonly List<Button> activeButtons = new();

        #region IEventSource

        public string SourceName => nameof(SocialDuelUI);
        public GameSystemType SystemType => GameSystemType.UI;

        #endregion

        #region Initialization

        private void Awake()
        {
            gameObject.SetActive(false);
        }
        
        public void Initialize(SocialDuelContext context)
        {
            this.context = context;

            InitializeResolveBars();
            BuildAbilityButtons();
        }

        private void InitializeResolveBars()
        {
            playerBar.Initialize(
                context.Player.Name,
                context.PlayerMaxResolve);

            playerBar.SetValue(context.PlayerCurrentResolve);

            npcBar.Initialize(
                context.Npc.Name,
                context.NpcMaxResolve);

            npcBar.SetValue(context.NpcCurrentResolve);
        }

        #endregion

        #region Ability Buttons

        private void BuildAbilityButtons()
        {
            ClearButtons();

            var abilities = context.Player.SocialAbilities;
            if (abilities == null)
                return;

            foreach (var ability in abilities)
            {
                var capturedAbility = ability;

                var button = Instantiate(
                    abilityButtonPrefab,
                    abilityButtonContainer);

                var label = button.GetComponentInChildren<TMP_Text>();
                if (label != null)
                    label.text = capturedAbility.AbilityName;

                button.onClick.AddListener(() =>
                {
                    SocialDuelSystem.Instance.PlayerUseAbility(capturedAbility);
                });

                activeButtons.Add(button);
            }
        }

        private void ClearButtons()
        {
            foreach (Transform child in abilityButtonContainer)
                Destroy(child.gameObject);

            activeButtons.Clear();
        }

        #endregion

        #region Resolve Animation

        public void AnimateResolveChange(EntityController entity, int newValue)
        {
            if (entity.IsPlayerControlled)
                StartCoroutine(AnimateBar(playerBar, newValue));
            else
                StartCoroutine(AnimateBar(npcBar, newValue));
        }

        private IEnumerator AnimateBar(ResolveBarUI bar, int target)
        {
            float duration = 0.3f;
            float timer = 0f;
            float start = bar.CurrentValue;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float value = Mathf.Lerp(start, target, timer / duration);
                bar.SetValue(Mathf.RoundToInt(value));
                yield return null;
            }

            bar.SetValue(target);
        }

        #endregion

        #region Visibility & Input

        public void Show()
        {
            gameObject.SetActive(true);
            ModalUISystem.Instance.OpenModal(this);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            ModalUISystem.Instance.CloseModal(this);
        }

        public void EnableInput(bool enabled)
        {
            foreach (var button in activeButtons)
            {
                if (button != null)
                    button.interactable = enabled;
            }
        }

        #endregion
    }
}