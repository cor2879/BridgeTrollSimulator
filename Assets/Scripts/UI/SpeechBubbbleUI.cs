using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class SpeechBubbleUI : MonoBehaviour, IModalUI
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

        [Header("Queue")]
        [SerializeField, ReadOnly]
        private readonly Queue<SpeechRequest> speechQueue = new();
        [SerializeField, ReadOnly]
        private bool isShowing = false;
        [SerializeField, ReadOnly]
        private SpeechRequest currentSpeechRequest;

        private RectTransform rectTransform;
        private Coroutine typingRoutine;
        private Coroutine popRoutine;
        private Coroutine waitRoutine;
        private Vector3 originalScale;

        private string fullText;
        private bool isTyping;
        private bool ignoreInputUntilRelease;

#region IEventSource

        public string SourceName 
        {
            get => $"{entity.Name}::{nameof(SpeechBubbleUI)}";
        }

        public GameSystemType SystemType => GameSystemType.UI;

#endregion

#region IModalUI

        public bool IsBlockingUI => false;

#endregion

        public bool IsTyping => isTyping;

        public event Action OnAdvanceRequested;

#region Initialization and Destroy

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

            if (ModalUISystem.Instance.IsBlockingWorldUI)
            {
                return;
            }

            if (ignoreInputUntilRelease)
            {
                if (!Input.anyKey)
                {
                    ignoreInputUntilRelease = false;
                }

                return;
            }

            if (!Input.anyKeyDown)
                return;

            Advance();
        }

        private void OnDestroy()
        {
            CleanupModal();
        }

#endregion

#region Public API

        public void Advance()
        {
            if (isTyping)
            {
                CompleteInstantly();
                return;
            }

            if (OnAdvanceRequested != null)
            {
                OnAdvanceRequested.Invoke();
            }
            Hide();
        }

        public void Show(string value,
                        SpeechBubbleMode mode = SpeechBubbleMode.Modal,
                        float duration = 2f)
        {
            speechQueue.Enqueue(new SpeechRequest
            {
                Text = value,
                Mode = mode,
                Duration = duration
            });

            #if UNITY_EDITOR
            Debug.Log($"{nameof(Show)}::Entity:{entity.SourceName}" +
                $"::\"{value}\" Queue Length: {speechQueue.Count} @Frame {Time.frameCount}");
            #endif

            if (!isShowing)
            {
                ProcessQueue();
            }
        }

        public void Hide()
        {
            #if UNITY_EDITOR

            Debug.Log($"{nameof(Hide)}::Entity:{entity.SourceName}" +
                $"::\"{fullText}\" @ Frame {Time.frameCount}");

            #endif

            if (!isShowing)
            {
                return;
            }

            StopTyping();

            if (popRoutine != null)
                StopCoroutine(popRoutine);

            popRoutine = StartCoroutine(
                Pop(
                    originalScale, 
                    Vector3.zero, 
                    popOutDuration, 
                    false));

            isShowing = false;
            ProcessQueue();
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

        private IEnumerator TypeRoutine()
        {
            isTyping = true;

            text.text = fullText;
            text.maxVisibleCharacters = 0;

            int totalChars = fullText.Length;

            for (int i = 0; i <= totalChars; i++)
            {
                text.maxVisibleCharacters = i;
                ResizeToFit();
                yield return new WaitForSeconds(typingSpeed);
            }

            isTyping = false;
        }

        private void CompleteInstantly()
        {
            #if UNITY_EDITOR
            Debug.Log($"{nameof(CompleteInstantly)}");
            #endif

            StopTyping();
            text.maxVisibleCharacters = fullText.Length;
            ResizeToFit();

            isTyping = false;
        }

        private void StopTyping()
        {
            if (typingRoutine != null)
            {
                StopCoroutine(typingRoutine);
                typingRoutine = null;
            }
        }

        private IEnumerator DelayedStartTyping()
        {
            yield return null;

            Canvas.ForceUpdateCanvases();

            ResizeToFit();
            StartTyping();
        }

#endregion

#region Layout

        private void ResizeToFit()
        {
            if (text == null)
                return;

            RectTransform textRect = text.rectTransform;
            text.ForceMeshUpdate();

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

        #endregion

        #region Queue Processing

        private void ProcessQueue()
        {
            if (waitRoutine != null)
            {
                return;
            }

            if (ModalUISystem.Instance.IsBlockingWorldUI)
            {
                waitRoutine = StartCoroutine(WaitForClearAndProcess());
                return;
            }

            if (!speechQueue.Any())
            {
                ModalUISystem.Instance.CloseModal(this);
                gameObject.SetActive(false);
                isShowing = false;
                return;
            }

            currentSpeechRequest = speechQueue.Dequeue();
            DisplaySpeech(currentSpeechRequest);
        }

        private IEnumerator WaitForClearAndProcess()
        {
            yield return new WaitUntil(() =>
                !ModalUISystem.Instance.IsBlockingWorldUI);
            
            waitRoutine = null;

            ProcessQueue();
        }

        private void DisplaySpeech(SpeechRequest request)
        {
            isShowing = true;

            fullText = request.Text;
            PositionBehind(entity.IsFacingRight);

            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);

            if (popRoutine != null)
            {
                StopCoroutine(popRoutine);
            }

            popRoutine = StartCoroutine(
                Pop(Vector3.zero, originalScale, popInDuration));

            ignoreInputUntilRelease = true;

            if (request.Mode == SpeechBubbleMode.Modal)
            {
                ModalUISystem.Instance.OpenModal(this);
            }

            StartCoroutine(DelayedStartTyping());
        }

        private void CleanupModal()
        {
            if (ModalUISystem.Instance != null)
            {
                ModalUISystem.Instance.CloseModal(this);
            }

            speechQueue.Clear();
            isShowing = false;
        }

        #endregion
    }
}