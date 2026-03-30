using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class SocialDuelUI : MonoBehaviour, IModalUI
    {
        [Header("Ability Buttons")]
        [SerializeField] private Transform abilityButtonContainer;
        [SerializeField] private Button abilityButtonPrefab;

        [Header("Resolve Bars")]
        [SerializeField] private ResolveBarUI playerBar;
        [SerializeField] private ResolveBarUI npcBar;

        [Header("UI Panels")]
        [SerializeField] private SocialDuelIntroUI introPanel;
        [SerializeField] private SocialDuelPreSummaryUI preSummaryPanel;
        [SerializeField] private SocialDuelResolutionUI outcomePanel;

        [SerializeField, ReadOnly]
        private int busy = 0;

        private SocialDuelContext context;
        private readonly List<Button> activeButtons = new();

        public int Busy { get => busy; private set => busy = value; }

        #region IEventSource

        public string SourceName => nameof(SocialDuelUI);
        public GameSystemType SystemType => GameSystemType.UI;

        #endregion

        #region IModalUI

        public bool IsBlockingUI => false;

        #endregion

        #region Initialization

        private void Awake()
        {
            HideAllChildren();
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
                context.Player.MaxResolve);

            playerBar.SetValue(context.Player.Resolve);

            npcBar.Initialize(
                context.Npc.Name,
                context.Npc.MaxResolve);

            npcBar.SetValue(context.Npc.Resolve);
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
                    label.text = capturedAbility.Name;

                button.onClick.AddListener(() =>
                {
                    SocialDuelSystem.Instance.HandleAbility(capturedAbility);
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

        public Coroutine AnimateResolveChange(EntityController entity, int newValue)
        {
            if (entity.IsPlayerControlled)
                return StartCoroutine(AnimateBar(playerBar, newValue));
            else
                return StartCoroutine(AnimateBar(npcBar, newValue));
        }

        private IEnumerator AnimateBar(ResolveBarUI bar, int target)
        {
            Busy++;

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
            Busy--;
        }

        #endregion

        #region Visibility & Input

        public void Show()
        {
            gameObject.SetActive(true);
            ShowDuelUI();
            ModalUISystem.Instance.OpenModal(this);
        }

        public void Hide()
        {
            HideAllChildren();
            ModalUISystem.Instance.CloseModal(this);
        }

        private void HideAllChildren()
        {
            abilityButtonContainer.gameObject.SetActive(false);
            playerBar.gameObject.SetActive(false);
            npcBar.gameObject.SetActive(false);
            introPanel.gameObject.SetActive(false);
            preSummaryPanel.gameObject.SetActive(false);
            outcomePanel.gameObject.SetActive(false);
        }

        public void EnableInput(bool enabled)
        {
            foreach (var button in activeButtons)
            {
                if (button != null)
                    button.interactable = enabled;
            }
        }

        public void ShowIntro(EntityController player, EntityController npc)
        {
            gameObject.SetActive(true);
            introPanel.Show(player, npc);
        }

        public void ShowDuelUI()
        {
            abilityButtonContainer.gameObject.SetActive(true);
            playerBar.gameObject.SetActive(true);
            npcBar.gameObject.SetActive(true);
        }

        #endregion
    }
}