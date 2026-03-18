using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class ResolveBarUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text nameLabel;
        [SerializeField] private TMP_Text resolveText;

        [Header("Visual References")]
        [SerializeField] private Image fillImage;
        [SerializeField] private Image backgroundImage;

        [Header("Colors")]
        [SerializeField] private Color baseBackgroundColor = new Color(0.12f, 0.12f, 0.12f);
        [SerializeField] private Color dangerBackgroundColor = new Color(0.4f, 0.05f, 0.05f);

        public float CurrentValue => Mathf.RoundToInt(slider.value);

        public void Initialize(string name, int maxResolve)
        {
            nameLabel.text = name;
            slider.maxValue = maxResolve;
            slider.value = maxResolve;

            UpdateVisuals();
        }

        public void SetValue(int value)
        {
            slider.value = value;
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            float percent = slider.value / slider.maxValue;

            resolveText.text = $"Resolve: {slider.value}/{slider.maxValue}";
            // Background shifts toward red as resolve drops
            backgroundImage.color = Color.Lerp(
                baseBackgroundColor,
                dangerBackgroundColor,
                1f - percent
            );
        }
    }
}