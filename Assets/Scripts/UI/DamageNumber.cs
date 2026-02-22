using UnityEngine;
using TMPro;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class DamageNumber : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 50f;
        [SerializeField] private float lifetime = 1f;

        private CanvasGroup canvasGroup;
        private TMP_Text text;
        private float timer;

        public void Initialize(int amount, bool isCrit)
        {
            text = GetComponent<TMP_Text>();
            canvasGroup = GetComponent<CanvasGroup>();

            text.text = amount > 0 ? $"-{amount}" : amount.ToString();
            var localScale = text.transform.localScale;
            text.transform.localScale = isCrit ? localScale * 1.5f : localScale;
        }

        private void Update()
        {
            // Float upward
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;

            // Fade out
            timer += Time.deltaTime;
            canvasGroup.alpha = 1f - (timer / lifetime);

            if (timer >= lifetime)
                Destroy(gameObject);
        }
    }
}