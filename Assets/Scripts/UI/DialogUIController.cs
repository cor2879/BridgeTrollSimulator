using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Policies;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class DialogUIController : MonoBehaviour, IEventSource
    {
        [Header("Panel Root")]
        [SerializeField] private GameObject panel;

        [Header("Log System")]
        [SerializeField] private Transform contentRoot;
        [SerializeField] private GameObject logEntryPrefab;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private int maxEntries = 50;

        private readonly List<GameObject> activeEntries = new();

        [Header("Choices")]
        [SerializeField] private GameObject choiceContainer;
        [SerializeField] private GameObject choiceButtonPrefab;

        private IDialogRenderable currentRenderable;
        private DialogNode currentStaticNode; // only needed for legacy static flow
        private EntityController initiator;
        private EntityController target;

        #region Typing Fields

        private Coroutine typingCoroutine;
        private TMP_Text currentTypingText;
        private string fullLine;
        private bool isTyping;
        private float typingDelay = 0.02f;

        #endregion

        #region IEventSource

        public string SourceName => nameof(DialogUIController);
        public GameSystemType SystemType => GameSystemType.UI;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            GameEventBus.Subscribe<DialogStartedEvent>(OnDialogStarted);
            GameEventBus.Subscribe<CombatStartedEvent>(OnCombatStarted);
            GameEventBus.Subscribe<CombatLogEvent>(OnCombatLog);
            GameEventBus.Subscribe<CombatEndedEvent>(OnCombatEnded);
            GameEventBus.Subscribe<SocialActionAttemptedEvent>(OnSocialActionAttempted);
            GameEventBus.Subscribe<ResolveChangedEvent>(OnResolveChanged);
            GameEventBus.Subscribe<TollPaidEvent>(OnTollPaid);
            GameEventBus.Subscribe<TollRefusedEvent>(OnTollRefused);
            GameEventBus.Subscribe<SystemMessageEvent>(OnSystemMessage);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<DialogStartedEvent>(OnDialogStarted);
            GameEventBus.Unsubscribe<CombatStartedEvent>(OnCombatStarted);
            GameEventBus.Unsubscribe<CombatLogEvent>(OnCombatLog);
            GameEventBus.Unsubscribe<CombatEndedEvent>(OnCombatEnded);
            GameEventBus.Unsubscribe<SocialActionAttemptedEvent>(OnSocialActionAttempted);
            GameEventBus.Unsubscribe<ResolveChangedEvent>(OnResolveChanged);
            GameEventBus.Unsubscribe<TollPaidEvent>(OnTollPaid);
            GameEventBus.Unsubscribe<TollRefusedEvent>(OnTollRefused);
            GameEventBus.Unsubscribe<SystemMessageEvent>(OnSystemMessage);
        }

        #endregion

        #region Event Handlers

        private void OnDialogStarted(DialogStartedEvent evt)
        {
            if (evt.RootNode == null)
            {
                Debug.LogWarning("DialogStartedEvent received with null RootNode.");
                return;
            }

            initiator = (EntityController)evt.Initiator;
            target = (EntityController)evt.Target;
            currentStaticNode = evt.RootNode;

            ShowRenderable(evt.RootNode);
        }

        private void OnCombatStarted(CombatStartedEvent evt)
        {
            ClearChoices();
            currentRenderable = null;
            panel.SetActive(true);
        }

        private void OnCombatEnded(CombatEndedEvent evt)
        {
            if (currentRenderable != null)
            {
                AdvanceNode();
            }
        }

        private void OnCombatLog(CombatLogEvent evt)
        {
            AppendLine("Combat", evt.Message, true);
        }

        private void OnResolveChanged(ResolveChangedEvent evt)
        {
            var direction = evt.Amount < 0 ? "loses" : "gains";

            AppendLine(
                "System",
                $"{evt.Entity.Name} {direction} {Mathf.Abs(evt.Amount)} Resolve.",
                false);
        }

        private void OnSocialActionAttempted(SocialActionAttemptedEvent evt)
        {
            string result = evt.Success ? "succeeded" : "failed";

            AppendLine(
                "System",
                $"{evt.Attacker.Name} attempted to {evt.ActionName} {evt.Target.Name} and {result}.",
                false);
        }        

        private void OnTollPaid(TollPaidEvent evt)
        {
            var initiator = evt.Initiator as EntityController;

            ShowSpeechBubble(initiator, initiator.DialogLibrary.payToll);
            
            ClearChoices();
            currentRenderable = null;

            EndDialog();
        }

        private void OnTollRefused(TollRefusedEvent evt)
        {
            var initiator = evt.Initiator as EntityController;

            ShowSpeechBubble(initiator, initiator.DialogLibrary.refuseToll);

            ClearChoices();
            currentRenderable= null;
        }

        private void OnSystemMessage(SystemMessageEvent evt)
        {
            AppendLine(evt.Sender.SourceName, evt.Message, true);
        }

        #endregion

        #region Dialog Flow

        private void AdvanceNode()
        {
            if (currentStaticNode == null ||
                currentStaticNode.Choices == null ||
                currentStaticNode.Choices.Count == 0)
            {
                EndDialog();
                return;
            }

            if (currentStaticNode.Choices.Count == 1)
            {
                currentStaticNode = currentStaticNode.Choices[0].NextNode;
                ShowRenderable(currentStaticNode);
            }
        }

        public void ShowNode(DialogNode node)
        {
            ClearChoices();
            ShowRenderable(node);
        }

        private void EndDialog()
        {
            initiator.SpeechBubble.OnAdvanceRequested -= HandleAdvance;
            target.SpeechBubble.OnAdvanceRequested -= HandleAdvance;

            // initiator?.SpeechBubble?.Hide();
            // target?.SpeechBubble?.Hide();

            currentRenderable = null;

            GameEventBus.Publish(
                new DialogEndedEvent(initiator, target, Time.frameCount));
        }

        private void ShowSpeechBubble(EntityController speaker, DialogNode node)
        {
            speaker.SpeechBubble.OnAdvanceRequested -= HandleAdvance;
            speaker.SpeechBubble.OnAdvanceRequested += HandleAdvance;
            speaker.SpeechBubble.Show(node.Text);
        }

        private void ShowSpeechBubble(EntityController speaker, string text)
        {
            speaker.SpeechBubble.OnAdvanceRequested -= HandleAdvance;
            speaker.SpeechBubble.OnAdvanceRequested += HandleAdvance;
            speaker.SpeechBubble.Show(text);            
        }

        private void HandleAdvance()
        {
            AdvanceNode();
        }

        #endregion

        #region Log System

        public void AppendLine(string speaker, string text, bool type = false)
        {
            ForceCompleteIfTyping();
            var entryObj = Instantiate(logEntryPrefab, contentRoot);
            entryObj.transform.SetAsLastSibling();
            var texts = entryObj.GetComponentsInChildren<TMP_Text>();

            if (texts.Length < 2)
            {
                Debug.LogError("LogEntry prefab must contain at least 2 TMP_Text components.");
                return;
            }

            texts[0].text = speaker;

            activeEntries.Add(entryObj);

            if (activeEntries.Count > maxEntries)
            {
                Destroy(activeEntries[0]);
                activeEntries.RemoveAt(0);
            }

            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;

            if (type)
            {
                currentTypingText = texts[1];
                currentTypingText.text = string.Empty;
                StartTyping(text);
            }
            else
            {
                texts[1].text = text;
            }
        }

        public void ClearLog()
        {
            foreach (var entry in activeEntries)
                Destroy(entry);

            activeEntries.Clear();
        }

        #endregion

        #region Typing

        private void StartTyping(string line)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            fullLine = line;
            typingCoroutine = StartCoroutine(TypeLine());
        }

        private IEnumerator TypeLine()
        {
            isTyping = true;

            foreach (char c in fullLine)
            {
                currentTypingText.text += c;
                yield return new WaitForSeconds(typingDelay);
            }

            isTyping = false;
        }

        private void ForceCompleteIfTyping()
        {
            if (!isTyping)
            {
                return;
            }

            CompleteLineInstantly();
        }

        private void CompleteLineInstantly()
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            currentTypingText.text = fullLine;
            isTyping = false;
        }

        #endregion

        #region Choices

        private void ClearChoices()
        {
            foreach (Transform child in choiceContainer.transform)
                Destroy(child.gameObject);

            choiceContainer.SetActive(false);
            ModalUISystem.Instance.CloseModal(this);
            Debug.Log("Choices Cleared");
        }

        public void ShowRuntimeNode(
            string text,
            List<GeneratedOption> options,
            EntityController initiator,
            EntityController target)
        {
            this.initiator = initiator;
            this.target = target;

            var runtimeNode = new RuntimeDialogNode
            {
                Text = text,
                Options = options
            };

            currentStaticNode = null;
            ShowRenderable(runtimeNode);
        }

        private void ShowRenderable(IDialogRenderable renderable)
        {
            currentRenderable = renderable;

            panel.SetActive(true);
            ClearChoices();

            if (!string.IsNullOrWhiteSpace(renderable.Text))
            {
                if (renderable is DialogNode staticNode)
                {
                    EntityController speaker = staticNode.SpeakerRole switch
                    {
                        DialogSpeakerRole.Initiator => initiator,
                        DialogSpeakerRole.Target => target,
                        DialogSpeakerRole.System => null,
                        _ => null
                    };

                    if (speaker != null)
                    {
                        ShowSpeechBubble(speaker, staticNode.Text);
                    }
                    else
                    {
                        AppendLine("System", staticNode.Text, true);
                    }
                }
                else
                {
                    // runtime node fallback (default to initiator)
                    ShowSpeechBubble(initiator, renderable.Text);
                }
            }

            RenderOptions(renderable.Options);
        }

        private void RenderOptions(List<GeneratedOption> options)
        {
            if (options == null || options.Count == 0)
                return;

            choiceContainer.SetActive(true);
            ModalUISystem.Instance.OpenModal(this);

            foreach (var option in options)
            {
                var buttonObj = Instantiate(choiceButtonPrefab, choiceContainer.transform);
                var buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
                buttonText.text = option.Label;

                var button = buttonObj.GetComponent<Button>();

                button.onClick.AddListener(() =>
                {
                    ClearChoices();
                    option.Execute(initiator, target);
                });
            }
        }

        #endregion
    }
}