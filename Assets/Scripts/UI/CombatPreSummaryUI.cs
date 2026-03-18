using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class CombatPreSummaryUI : ModalUIBase, IEventSource
    {
        [Header("Root")]
        [SerializeField]
        private GameObject panelRoot;

        [Header("Containers")]
        [SerializeField]
        private Transform teamAContainer;
        [SerializeField]
        private Transform teamBContainer;

        [Header("UI Elements")]
        [SerializeField]
        private TMP_Text resultText;
        [SerializeField] 
        private TMP_Text continueText;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject combatantSlotPrefab;

        private bool awaitingInput;
        private CombatConfirmedEvent combatConfirmedEvent;

        public override string SourceName => nameof(CombatPreSummaryUI);
        public override GameSystemType SystemType => GameSystemType.UI;
        public override bool IsBlockingUI => true;

        private void Awake()
        {
            panelRoot.SetActive(false);
        }

        public void Show(
            IEnumerable<EntityController> teamA,
            IEnumerable<EntityController> teamB,
            EntityController firstMover,
            CombatConfirmedEvent evt)
        {
            ClearContainers();

            PopulateTeam(teamA, teamAContainer);
            PopulateTeam(teamB, teamBContainer);

            resultText.text =
                $"{firstMover.Name} has First Initiative!";

            continueText.text = "Press Any Key to Begin";
            ShowModal(panelRoot);
            combatConfirmedEvent = evt;
            panelRoot.transform.localScale = Vector3.zero;
            StartCoroutine(PopIn());
        }

        private void PopulateTeam(
            IEnumerable<EntityController> team,
            Transform container)
        {
            foreach (var entity in team)
            {
                var slotObj = Instantiate(combatantSlotPrefab, container);

                var image = slotObj.transform
                    .Find("Portrait")
                    .GetComponent<Image>();

                var nameText = slotObj.transform
                    .Find("NameText")
                    .GetComponent<TMP_Text>();

                image.sprite = entity.BattleIntroSprite ?? entity.SpriteRenderer.sprite;
                image.preserveAspect = true;
                nameText.text = entity.Name;
            }
        }

        private void ClearContainers()
        {
            foreach (Transform child in teamAContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in teamBContainer)
            {
                Destroy(child.gameObject);
            }
        }

        private void Update()
        {
            if (!awaitingInput)
            {
                return;
            }

            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                awaitingInput = false;
                HideModal(panelRoot);
                GameEventBus.Publish(
                    new CombatPreSummaryConfirmedEvent(
                        combatConfirmedEvent.Initiator,
                        combatConfirmedEvent.Target,
                        Time.frameCount));
                combatConfirmedEvent = null;
            }
        }

        private IEnumerator PopIn()
        {
            float duration = 0.50f;
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;

                panelRoot.transform.localScale =
                    Vector3.Lerp(Vector3.zero, Vector3.one, t);

                yield return null;
            }

            panelRoot.transform.localScale = Vector3.one;
            awaitingInput = true;
        }
    }
}