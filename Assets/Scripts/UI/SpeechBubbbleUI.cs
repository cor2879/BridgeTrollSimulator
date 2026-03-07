using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class SpeechBubbleUI : MonoBehaviour, IEventSource
    {
        [Header("Entity")]
        [SerializeField] private EntityController entity;

        [Header("Visuals")]
        [SerializeField] private Image bubbleImage;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Sprite leftSprite;
        [SerializeField] private Sprite rightSprite;
        [SerializeField] private float popInDuration = 0.12f;
        [SerializeField] private float popOutDuration = 0.08f;

        [Header("Layout")]
        [SerializeField, ReadOnly] private Vector2 baseOffset;
        [SerializeField] private float paddingX = 80f;
        [SerializeField] private float paddingY = 60f;
        [SerializeField] private float maxWidth = 300f;

        [Header("Typing")]
        [SerializeField] private float typingSpeed = 0.02f;

        private RectTransform rectTransform;
        private Coroutine typingRoutine;
        private Coroutine popRoutine;
        private Coroutine autoDismissRoutine;
        private Vector3 originalScale;

        private string fullText;
        private bool isTyping;
        private bool waitingForAdvance;
        private bool ignoreInputUntilRelease;

#region IEventSource

        public string SourceName 
        {
            get => $"{entity.Name}::{nameof(SpeechBubbleUI)}";
        }

        public GameSystemType SystemType => GameSystemType.UI;

#endregion

        public bool IsTyping => isTyping;
        public bool IsWaitingForAdvance => waitingForAdvance;

        public event Action OnAdvanceRequested;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            originalScale = transform.localScale;
            baseOffset = transform.localPosition;
            transform.localScale = Vector3.zero;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!gameObject.activeSelf)
                return;

            if (ignoreInputUntilRelease)
            {
                if (!Input.anyKey)
                {
                    ignoreInputUntilRelease = false;
                    return;
                }
            } 

            if (!Input.anyKeyDown)
            {
                return;
            }

            if (isTyping)
            {
                CompleteInstantly();
                return;
            }

            if (waitingForAdvance)
            {
                waitingForAdvance = false;
                OnAdvanceRequested?.Invoke();
                Hide();
            }
        }

        private void OnEnable()
        {

        }

#region Public API

        public void Show(string value,
                        SpeechBubbleMode mode = SpeechBubbleMode.Modal,
                        float duration = 2f)
        {
            fullText = value;

            PositionBehind(entity.IsFacingRight);
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);

            if (popRoutine != null)
                StopCoroutine(popRoutine);

            popRoutine = StartCoroutine(
                Pop(
                    Vector3.zero, 
                    originalScale, 
                    popInDuration));

            ignoreInputUntilRelease = true;
            if (mode == SpeechBubbleMode.Modal)
            {
                ModalUISystem.Instance.OpenModal(this);
                StartCoroutine(StartTypingNextFrame());
            }
            else
            {
                StartCoroutine(StartTypingNextFrame());
                autoDismissRoutine = StartCoroutine(AutoDismiss(duration));
            }
        }

        public void Hide()
        {
            Debug.Log($"{nameof(Hide)}::Entity:{entity.Name} @ Frame {Time.frameCount}");
            if (!gameObject.activeSelf)
            {
                return;
            }

            StopTyping();
            waitingForAdvance = false;

            if (popRoutine != null)
                StopCoroutine(popRoutine);

            popRoutine = StartCoroutine(
                Pop(
                    originalScale, 
                    Vector3.zero, 
                    popOutDuration, 
                    false));

            StartCoroutine(CloseModalAfterKeyRelease());
        }

#endregion

#region Positioning

        private void PositionBehind(bool isFacingRight)
        {
            float x = Mathf.Abs(baseOffset.x);

            // Facing right → bubble on right side (tail pointing down-left)
            float directionalX = isFacingRight ? -x : x;

            rectTransform.anchoredPosition = new Vector2(
                directionalX,
                baseOffset.y);

            if (bubbleImage != null)
            {
                bubbleImage.sprite = isFacingRight ? rightSprite : leftSprite;
            }
        }

#endregion

#region Typing Logic

        private void StartTyping()
        {
            StopTyping();
            typingRoutine = StartCoroutine(TypeRoutine());
        }

        private IEnumerator StartTypingNextFrame()
        {
            yield return null;
            StartTyping();
        }

        private IEnumerator TypeRoutine()
        {
            isTyping = true;
            waitingForAdvance = false;

            text.text = "";

            foreach (char c in fullText)
            {
                text.text += c;
                ResizeToFit();
                yield return new WaitForSeconds(typingSpeed);
            }

            isTyping = false;
            waitingForAdvance = true;
        }

        private void CompleteInstantly()
        {
            StopTyping();
            text.text = fullText;
            ResizeToFit();

            isTyping = false;
            waitingForAdvance = true;
        }

        private void StopTyping()
        {
            if (typingRoutine != null)
            {
                StopCoroutine(typingRoutine);
                typingRoutine = null;
            }
        }

#endregion

#region Layout

        private void ResizeToFit()
        {
            if (text == null)
                return;

            RectTransform textRect = text.rectTransform;

            // Step 1: Get natural width (no wrapping yet)
            Vector2 unconstrained = text.GetPreferredValues(
                text.text,
                Mathf.Infinity,
                Mathf.Infinity
            );

            float finalWidth = Mathf.Min(unconstrained.x, maxWidth);

            // Step 2: Apply width to text rect so wrapping works
            textRect.sizeDelta = new Vector2(finalWidth, 0f);

            text.ForceMeshUpdate();

            // Step 3: Now get height based on constrained width
            Vector2 wrapped = text.GetPreferredValues(
                text.text,
                finalWidth,
                Mathf.Infinity
            );

            textRect.sizeDelta = new Vector2(
                finalWidth,
                wrapped.y
            );

            // Step 4: Size bubble around it
            rectTransform.sizeDelta = new Vector2(
                finalWidth + paddingX,
                wrapped.y + paddingY
            );
        }

        private IEnumerator Pop(Vector3 from, Vector3 to, float duration, bool deactivateAfter = false)
        {
            float t = 0f;

            transform.localScale = from;

            while (t < duration)
            {
                t += Time.deltaTime;
                float normalized = t / duration;

                // Ease out cubic (snappy but clean)
                float eased = 1f - Mathf.Pow(1f - normalized, 3f);

                transform.localScale = Vector3.LerpUnclamped(from, to, eased);
                yield return null;
            }

            transform.localScale = to;

            if (deactivateAfter)
                gameObject.SetActive(false);
        }

        private IEnumerator AutoDismiss(float delay)
        {
            yield return new WaitUntil(() => !isTyping);
            yield return new WaitForSeconds(delay);
            Hide();
        }

        private IEnumerator WaitForKeyUpThenDoAction(Action action)
        {
            yield return new WaitUntil(() => !Input.anyKey);

            action?.Invoke();
        }

        private IEnumerator CloseModalAfterKeyRelease()
        {
            yield return new WaitUntil(() => !Input.anyKey);

            ModalUISystem.Instance.CloseModal(this);
            gameObject.SetActive(false);
        }

        #endregion
    }
}