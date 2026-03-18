using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Audio;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class GoldPopupUI : MonoBehaviour
    {
        private struct GoldRequest
        {
            public int Start;
            public int End;
            public int Delta;
        }

        [SerializeField] private TMP_Text currentGoldText;
        [SerializeField] private TMP_Text deltaText;
        [SerializeField] private SpriteRenderer coinImage;

        [SerializeField] private float deltaRiseDistance = 20f;
        [SerializeField] private float deltaRiseDuration = 0.5f;
        [SerializeField] private float lingerTime = 0.4f;

        [SerializeField, ReadOnly]
        private bool isPlaying;

        private readonly Queue<GoldRequest> queue = new();
        private Coroutine activeRoutine;
        private EntityController entity;

        private void Awake()
        {
            entity = GetComponent<EntityController>();
            gameObject.SetActive(false);
        }

        public void Play(int startGold, int endGold, int delta)
        {
            queue.Enqueue(new GoldRequest
            {
                Start = startGold,
                End = endGold,
                Delta = delta
            });

            if (!isPlaying)
            {
                ProcessQueue();
            }
        }

        public void ProcessQueue()
        {
            if (queue.Count == 0)
            {
                isPlaying = false;
                gameObject.SetActive(false);
                return;
            }

            isPlaying = true;
            var request = queue.Dequeue();

            if (activeRoutine != null)
                StopCoroutine(activeRoutine);

            gameObject.SetActive(true);

            deltaText.gameObject.SetActive(true);
            deltaText.transform.localPosition = Vector3.zero;

            currentGoldText.text = request.Start.ToString();
            deltaText.text = $"+{request.Delta}";

            activeRoutine = StartCoroutine(
                AnimateSequence(request.Start, request.End));
        }

        private IEnumerator AnimateSequence(int start, int end)
        {
            GameEventBus.Publish(
                new SoundEffectEvent(
                    entity,
                    AudioSystem.Library.coins,
                    Time.frameCount));

            yield return StartCoroutine(AnimateDeltaRise());
            yield return StartCoroutine(TypeGoldIncrease(start, end));
            yield return new WaitForSeconds(lingerTime);

            ProcessQueue();
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
            int delta = end - start;
            float duration = Mathf.Clamp(delta * 0.02f, 0.3f, 1.0f);

            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                int value = Mathf.RoundToInt(Mathf.Lerp(start, end, t));
                currentGoldText.text = value.ToString();

                yield return null;
            }

            currentGoldText.text = end.ToString();
        }

        public void SetActive(bool active)
        {
            this.gameObject.SetActive(active);
        }
    }
}