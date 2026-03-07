using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog;
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

        private DialogNode currentNode;
        private RuntimeDialogNode currentRuntimeNode;
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

            initiator = evt.Initiator;
            target = evt.Target;
            currentNode = evt.RootNode;

            panel.SetActive(true);
            ClearChoices();

            ShowCharacterSpeech(currentNode);
        }

        private void OnCombatStarted(CombatStartedEvent evt)
        {
            ClearChoices();
            currentNode = null;
            panel.SetActive(true);
        }

        private void OnCombatEnded(CombatEndedEvent evt)
        {
            if (currentNode != null)
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
            ShowSpeechBubble(evt.Initiator, evt.Initiator.DialogLibrary.payToll);
            
            ClearChoices();
            currentRuntimeNode = null;

            EndDialog();
        }

        private void OnTollRefused(TollRefusedEvent evt)
        {
            ShowSpeechBubble(evt.Initiator, evt.Initiator.DialogLibrary.refuseToll);

            ClearChoices();
            currentRuntimeNode = null;
        }

        #endregion

        #region Dialog Flow

        private void AdvanceNode()
        {
            if (currentNode == null ||
                currentNode.Choices == null || 
                currentNode.Choices.Count == 0)
            {
                EndDialog();
                return;
            }

            if (currentNode.Choices.Count == 1)
            {
                currentNode = currentNode.Choices[0].NextNode;
                ShowNode(currentNode);
            }
        }

        private void ShowNode(DialogNode node)
        {
            ClearChoices();
            ShowCharacterSpeech(node);
        }

        private void EndDialog()
        {
            initiator.SpeechBubble.OnAdvanceRequested -= HandleAdvance;
            target.SpeechBubble.OnAdvanceRequested -= HandleAdvance;

            // initiator?.SpeechBubble?.Hide();
            // target?.SpeechBubble?.Hide();

            currentNode = null;

            GameEventBus.Publish(
                new DialogEndedEvent(initiator, target, Time.frameCount));
        }

        private void ShowCharacterSpeech(DialogNode node)
        {
            EntityController speaker = null;

            switch (node.SpeakerRole)
            {
                case DialogSpeakerRole.Initiator:
                    speaker = initiator;
                    // target.SpeechBubble.Hide();
                    break;

                case DialogSpeakerRole.Target:
                    speaker = target;
                    // initiator.SpeechBubble.Hide();
                    break;

                case DialogSpeakerRole.System:
                    AppendLine("System", node.Text, true);
                    return;
            }

            if (speaker != null && speaker.SpeechBubble != null)
            {
                ShowSpeechBubble(speaker, node);
            }

            ShowChoicesIfNeeded();
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

            ShowChoicesIfNeeded();
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

        private void ShowChoicesIfNeeded()
        {
            if (currentNode != null)
            {
                if (currentNode.Choices == null || currentNode.Choices.Count <= 1)
                    return;

                choiceContainer.SetActive(true);
                CreateChoiceButtons(currentNode);
                return;
            }

            if (currentRuntimeNode != null)
            {
                choiceContainer.SetActive(true);
                ModalUISystem.Instance.OpenModal(this);
                CreateRuntimeChoiceButtons(currentRuntimeNode);
            }
        }

        private void CreateChoiceButtons(DialogNode node)
        {
            for (int i = 0; i < node.Choices.Count; i++)
            {
                var choice = node.Choices[i];
                int index = i;

                var buttonObj = Instantiate(choiceButtonPrefab, choiceContainer.transform);
                var buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
                buttonText.text = choice.ChoiceText;

                var button = buttonObj.GetComponent<Button>();
                bool available = IsChoiceAvailable(choice);

                button.interactable = available;

                if (available)
                {
                    button.onClick.AddListener(() => Choose(index));
                }
                else
                {
                    buttonText.color = Color.gray;
                }
            }
        }

        private void CreateRuntimeChoiceButtons(RuntimeDialogNode node)
        {
            for (int i = 0; i < node.Options.Count; i++)
            {
                var option = node.Options[i];
                Debug.Log($"{option.Label}");

                var buttonObj = Instantiate(choiceButtonPrefab, choiceContainer.transform);
                var buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
                buttonText.text = option.Label;

                var button = buttonObj.GetComponent<Button>();

                button.onClick.AddListener(() =>
                {
                    currentRuntimeNode = null;
                    option.Execute(initiator, target);
                });
            }
        }

        private void Choose(int index)
        {
            if (currentNode == null || index < 0 || index >= currentNode.Choices.Count)
                return;

            var choice = currentNode.Choices[index];

            if (!IsChoiceAvailable(choice))
                return;

            var nextNode = choice.NextNode;

            // Advance static dialog FIRST
            if (nextNode == null)
            {
                EndDialog();
            }
            else
            {
                currentNode = nextNode;
                ShowNode(currentNode);
            }

            // THEN execute actions (which may trigger runtime branching)
            foreach (var action in choice.Actions)
            {
                action.Execute(initiator, target);
            }
        }

        private bool IsChoiceAvailable(DialogChoice choice)
        {
            foreach (var action in choice.Actions)
            {
                if (!action.CanExecute(initiator, target))
                    return false;
            }

            return true;
        }

        public void ShowRuntimeNode(
            string text,
            List<GeneratedOption> options,
            EntityController initiator,
            EntityController target)
        {
            this.initiator = initiator;
            this.target = target;

            currentNode = null; // important
            currentRuntimeNode = new RuntimeDialogNode
            {
                Text = text,
                Options = options
            };

            panel.SetActive(true);
            ClearChoices();

            if (!string.IsNullOrWhiteSpace(text))
            {
                ShowSpeechBubble(initiator, text);
            }
            else
            {
                ShowChoicesIfNeeded();
            }

            if (options == null || options.Count == 0)
            {
                Debug.LogWarning("Runtime options list empty.");
                return;
            }
        }

        #endregion
    }
}