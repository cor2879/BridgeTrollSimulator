using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class CombatUIController : MonoBehaviour
    {
        [Header("Ability UI")]
        [SerializeField] private GameObject abilityContainer;
        [SerializeField] private GameObject abilityButtonPrefab;

        private CombatSystem combatSystem;
        private readonly List<Button> abilityButtons = new();

        public void Initialize(CombatSystem system, EntityController player)
        {
            combatSystem = system;

            ClearAbilityButtons();

            foreach (var ability in player.Abilities)
            {
                Debug.Log(ability.Name);
                CreateAbilityButton(player, ability);
            }
        }

        private void CreateAbilityButton(EntityController player, Ability ability)
        {
            var buttonObj = Instantiate(abilityButtonPrefab, abilityContainer.transform);

            var button = buttonObj.GetComponent<Button>();
            var text = buttonObj.GetComponentInChildren<TMP_Text>();

            text.text = $"{ability.Name} ({ability.StaminaCost})";

            button.onClick.AddListener(() =>
            {
                combatSystem.PlayerUseAbility(ability);
            });

            abilityButtons.Add(button);
        }

        private void ClearAbilityButtons()
        {
            foreach (Transform child in abilityContainer.transform)
            {
                Destroy(child.gameObject);
            }

            abilityButtons.Clear();
        }

        public void Show()
        {
            abilityContainer.SetActive(true);
        }

        public void Hide()
        {
            abilityContainer.SetActive(false);
        }

        public void EnableInput(bool enabled)
        {
            foreach (var button in abilityButtons)
            {
                button.interactable = enabled;
            }
        }
    }
}