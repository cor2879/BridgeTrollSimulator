using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
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
        private readonly List<(Button button, Ability ability)> abilityButtons = new();
        private EntityController player;
        [SerializeField, ReadOnly]
        private bool isVisible = false;
        private Coroutine activeAnimation;

        public void Initialize(CombatSystem system, EntityController player)
        {
            EnableInput(false);
            combatSystem = system;
            this.player = player;
            ClearAbilityButtons();

            var abilities = player.ActiveAbilities
                .Where(a => a.Name != "Concede")
                .OrderBy(a => a.StaminaCost)
                .ToList();
            
            var concede = player.ActiveAbilities
                .FirstOrDefault(a => a.Name == "Concede");

            if (concede != null)
            {
                abilities.Add(concede);
            }

            foreach (var ability in abilities)
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

            abilityButtons.Add((button, ability));
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
            Debug.Log($"{nameof(CombatUIController.Show)}");
            StartCoroutine(PopIn());
        }

        public void Hide()
        {
            StartCoroutine(PopOut());
        }

        public void EnableInput(bool enabled)
        {
            foreach (var (button, ability) in abilityButtons)
            {
                button.interactable = enabled && ability.CanExecute(player);
            }

            if (activeAnimation != null)
            {
                StopCoroutine(activeAnimation);
            }

            if (enabled && !isVisible)
            {
                activeAnimation = StartCoroutine(PopIn());
                isVisible = true;
            }
            else if (!enabled && isVisible)
            {
                activeAnimation = StartCoroutine(PopOut());
                isVisible = false;
            }
        }

        private IEnumerator PopIn()
        {
            abilityContainer.SetActive(true);

            float duration = 0.35f;
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;

                abilityContainer.transform.localScale =
                    Vector3.Lerp(Vector3.zero, Vector3.one, t);

                yield return null;
            }

            abilityContainer.transform.localScale = Vector3.one;
        }

        private IEnumerator PopOut()
        {
            float duration = 0.35f;
            float timer = 0f;

            Vector3 startScale = Vector3.one;
            Vector3 endScale = Vector3.zero;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;

                abilityContainer.transform.localScale =
                    Vector3.Lerp(startScale, endScale, t);

                yield return null;
            }

            abilityContainer.transform.localScale = Vector3.zero;

            abilityContainer.SetActive(false);
        }
    }
}