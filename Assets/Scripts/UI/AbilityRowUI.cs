using UnityEngine;
using TMPro;
using UnityEngine.UI;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class AbilityRowUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text tierText;

        [SerializeField] private Image background;
        [SerializeField] private Image rarityBorder;

        [SerializeField] private Button button;

        private AbilityNode node;
        private System.Action<AbilityNode> onClicked;

        public AbilityNode Node => node;

        public void Initialize(AbilityNode node, System.Action<AbilityNode> onClicked)
        {
            this.node = node;
            this.onClicked = onClicked;

            var ability = node.ability;

            nameText.text = ability?.Name ?? "Unknown";

            if (descriptionText != null)
                descriptionText.text = ability?.Description ?? "";

            if (tierText != null)
                tierText.text = $"Tier {node.tier}";

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClicked?.Invoke(node));

            ApplyRarityVisuals(ability);
        }

        public void SetSelected(bool selected)
        {
            if (background != null)
            {
                background.color = selected
                    ? new Color(0.25f, 0.25f, 0.7f, 1f)
                    : Color.black;
            }
        }

        private void ApplyRarityVisuals(Ability ability)
        {
            if (rarityBorder == null || ability == null)
                return;

            rarityBorder.color = ability.Rarity switch
            {
                AbilityRarity.Common => new Color(0.7f, 0.7f, 0.7f),
                AbilityRarity.Rare => new Color(0.3f, 0.5f, 1f),
                AbilityRarity.Legendary => new Color(1f, 0.75f, 0.2f),
                _ => Color.white
            };
        }
    }
}