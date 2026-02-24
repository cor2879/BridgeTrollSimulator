using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class GoldPopupUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentGoldText;
        [SerializeField] private TMP_Text deltaText;
        [SerializeField] private SpriteRenderer coinImage;

        [SerializeField] private float deltaRiseDistance = 20f;
        [SerializeField] private float deltaRiseDuration = 0.5f;
        [SerializeField] private float typeSpeed = 0.03f;
        [SerializeField] private float lingerTime = 0.4f;

        private Coroutine activeRoutine;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Play(int startGold, int endGold, int delta)
        {
            Debug.Log("Play GoldUI");
            if (activeRoutine != null)
                StopCoroutine(activeRoutine);

            gameObject.SetActive(true);

            deltaText.gameObject.SetActive(true);
            deltaText.transform.localPosition = Vector3.zero;

            currentGoldText.text = startGold.ToString();
            deltaText.text = $"+{delta}";

            activeRoutine = StartCoroutine(AnimateSequence(startGold, endGold));
        }

        private IEnumerator AnimateSequence(int start, int end)
        {
            yield return StartCoroutine(AnimateDeltaRise());
            yield return StartCoroutine(TypeGoldIncrease(start, end));
            yield return new WaitForSeconds(lingerTime);

            gameObject.SetActive(false);
        }

        private IEnumerator AnimateDeltaRise()
        {
            float timer = 0f;
            Vector3 startPos = deltaText.transform.localPosition;
            Vector3 endPos = startPos + Vector3.up * deltaRiseDistance;

            while (timer < deltaRiseDuration)
            {
                timer += Time.deltaTime;
                float t = timer / deltaRiseDuration;

                deltaText.transform.localPosition =
                    Vector3.Lerp(startPos, endPos, t);

                yield return null;
            }

            deltaText.gameObject.SetActive(false);
        }

        private IEnumerator TypeGoldIncrease(int start, int end)
        {
            int current = start;

            while (current < end)
            {
                current++;
                currentGoldText.text = current.ToString();
                yield return new WaitForSeconds(typeSpeed);
            }

            currentGoldText.text = end.ToString();
        }

        public void SetActive(bool active)
        {
            this.gameObject.SetActive(active);
        }
    }
}