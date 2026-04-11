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
        private Vector3 drift;

        public void Initialize(string value, bool isCrit, Color color)
        {
            text = GetComponent<TMP_Text>();
            canvasGroup = GetComponent<CanvasGroup>();

            text.text = value;
            text.color = color;

            var localScale = text.transform.localScale;
            text.transform.localScale = isCrit ? localScale * 1.5f : localScale;
        }

        private void Awake()
        {
            drift = new Vector3(Random.Range(-0.3f, 0.3f), 1f, 0f);
        }

        private void Update()
        {
            // Float upward
            transform.position += drift * moveSpeed * Time.deltaTime;

            // Fade out
            timer += Time.deltaTime;
            canvasGroup.alpha = 1f - (timer / lifetime);

            if (timer >= lifetime)
                Destroy(gameObject);
        }
    }
}