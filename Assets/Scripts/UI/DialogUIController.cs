using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class DialogUIController : MonoBehaviour
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
        private EntityController initiator;
        private EntityController target;

        #region Typing Fields

        private Coroutine typingCoroutine;
        private TMP_Text currentTypingText;
        private string fullLine;
        private bool isTyping;
        private float typingDelay = 0.02f;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            GameEventBus.Subscribe<DialogStartedEvent>(OnDialogStarted);
            GameEventBus.Subscribe<CombatStartedEvent>(OnCombatStarted);
            GameEventBus.Subscribe<CombatLogEvent>(OnCombatLog);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<DialogStartedEvent>(OnDialogStarted);
            GameEventBus.Unsubscribe<CombatStartedEvent>(OnCombatStarted);
            GameEventBus.Unsubscribe<CombatLogEvent>(OnCombatLog);
        }

        private void Update()
        {
            if (currentNode == null)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isTyping)
                    CompleteLineInstantly();
                else
                    AdvanceNode();
            }
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

            AppendLine(currentNode.Speaker, currentNode.Text, true);
        }

        private void OnCombatStarted(CombatStartedEvent evt)
        {
            // Keep panel active – it becomes the combat log
            panel.SetActive(true);
            AppendLine("System", "Combat started!", true);
        }

        private void OnCombatLog(CombatLogEvent evt)
        {
            AppendLine("Combat", evt.Message, true);
        }

        #endregion

        #region Dialog Flow

        private void AdvanceNode()
        {
            if (currentNode.Choices == null || currentNode.Choices.Count == 0)
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
            AppendLine(node.Speaker, node.Text, true);
        }

        private void EndDialog()
        {
            currentNode = null;

            GameEventBus.Publish(
                new DialogEndedEvent(initiator, target, Time.frameCount));
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
        }

        private void ShowChoicesIfNeeded()
        {
            if (currentNode == null)
                return;

            if (currentNode.Choices == null || currentNode.Choices.Count <= 1)
                return;

            choiceContainer.SetActive(true);
            CreateChoiceButtons(currentNode);
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

        private void Choose(int index)
        {
            if (currentNode == null || index < 0 || index >= currentNode.Choices.Count)
                return;

            var choice = currentNode.Choices[index];

            if (!IsChoiceAvailable(choice))
                return;

            foreach (var action in choice.Actions)
                action.Execute(initiator, target);

            currentNode = choice.NextNode;

            if (currentNode == null)
                EndDialog();
            else
                ShowNode(currentNode);
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

        #endregion
    }
}