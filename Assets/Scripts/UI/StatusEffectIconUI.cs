using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class StatusEffectIconUI : MonoBehaviour
    {
        private Vector3 baseScale;

        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text durationText;
        private int duration;

        public string EffectId { get; private set; }

        public void Awake()
        {
            baseScale = transform.localScale;
        }

        public void Initialize(string effectId, Sprite icon, Color color, int duration)
        {
            EffectId = effectId;

            iconImage.sprite = icon;
            iconImage.color = color;

            SetDuration(duration);
        }

        public void SetDuration(int duration)
        {
            this.duration = duration;
            UpdateDurationText();
        }

        public void DecrementDuration()
        {
            duration--;
            UpdateDurationText();
        }

        private void UpdateDurationText()
        {
            durationText.text = duration > 0 ? duration.ToString() : "";
        }


#region Effects

        public void PlayTickFeedback(EffectDefinition effect)
        {
            if (effect.IconColor != default)
            {
                StartCoroutine(Flash(effect.IconColor));
            }

            StartCoroutine(Pulse());
        }

        public void ExpireAndDestroy()
        {
            iconImage.color = Color.gray;
            StartCoroutine(FadeOutAndDestroy());
        }

#endregion

#region Coroutines

        private IEnumerator Pulse()
        {
            float duration = 0.1f;
            float t = 0f;

            while (t < duration)
            {
                t += Time.deltaTime;
                float scale = Mathf.Lerp(1f, 1.2f, t / duration);
                transform.localScale = baseScale * scale;
                yield return null;
            }

            t = 0f;

            while (t < duration)
            {
                t += Time.deltaTime;
                float scale = Mathf.Lerp(1.2f, 1f, t / duration);
                transform.localScale = baseScale * scale;
                yield return null;
            }
        }

        private IEnumerator Flash(Color color)
        {
            var originalColor = iconImage.color;
            iconImage.color = color;
            yield return new WaitForSeconds(0.1f);
            iconImage.color = originalColor;
        }

        private IEnumerator FadeOutAndDestroy()
        {
            float duration = 0.2f;
            float interval = 0f;

            while (interval < duration)
            {
                interval += Time.deltaTime;
                float normalized = interval / duration;

                transform.localScale = Vector3.Lerp(baseScale, Vector3.zero, normalized);
                yield return null;
            }

            Destroy(gameObject);
        }

#endregion
    }
}