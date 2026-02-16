using System.Collections;
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
        [SerializeField] 
        private GameObject panel;
        [SerializeField]
        private TMP_Text speakerNameText;
        [SerializeField]
        private TMP_Text dialogText;

        [SerializeField]
        private Transform choiceContainer;
        [SerializeField]
        private GameObject choiceButtonPrefab;

        private DialogNode currentNode;
        private DialogNode rootNode;
        private EntityController initiator;
        private EntityController target;

        #region Typing Enhancement Fields

        private Coroutine typingCoroutine;
        private bool isTyping;
        private string fullLine;
        private float typingDelay = 0.02f;

        #endregion

        private void OnEnable()
        {
            GameEventBus.Subscribe<DialogStartedEvent>(OnDialogStarted);
            GameEventBus.Subscribe<CombatStartedEvent>(OnCombatStarted);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<DialogStartedEvent>(OnDialogStarted);
        }

        private void OnDialogStarted(DialogStartedEvent evt)
        {
            Debug.Log(evt.ToString());

            if (evt.RootNode is null)
            {
                Debug.LogWarning("DialogStartedEvent received with Null Sequence");
            }

            currentNode = evt.RootNode;
            initiator = evt.Initiator;
            target = evt.Target;

            ShowDialog(currentNode);
        }

        private void OnCombatStarted(CombatStartedEvent evt)
        {
            panel.SetActive(false);
        }

        private void Update()
        {
            if (currentNode is null) 
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isTyping)
                {
                    CompleteLineInstantly();
                }
                else
                {
                    AdvanceNode();
                }
            }
        }

        public void ShowDialog(DialogNode node)
        {
            panel.SetActive(true);
            speakerNameText.text = node.Speaker;
            StartTyping(node.Text);

            ClearChoices();
        }

        private void EndDialog()
        {
            panel.SetActive(false);

            GameEventBus.Publish(
                new DialogEndedEvent(initiator, target, Time.frameCount));

            currentNode = null;
        }

        private void AdvanceNode()
        {
            if (currentNode.Choices is null || currentNode.Choices.Count == 0)
            {
                EndDialog();
                return;
            }

            if (currentNode.Choices.Count == 1)
            {
                currentNode = currentNode.Choices[0].NextNode;
                ShowDialog(currentNode);
                return;
            }
        }

        private void ClearChoices()
        {
            foreach (Transform child in choiceContainer)
            {
                Destroy(child.gameObject);
            }
        }

        private void CreateChoiceButtons(DialogNode node)
        {
            for (var i = 0; i < node.Choices.Count; i++)
            {
                var choice = node.Choices[i];
                var index = i;

                var buttonObj = Instantiate(choiceButtonPrefab, choiceContainer);
                var buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
                buttonText.text = choice.ChoiceText;

                var button = buttonObj.GetComponent<Button>();
                bool available = IsChoiceAvailable(choice);

                Debug.Log($"{choice}.available == {available}");
                button.interactable = available;

                if (available)
                {
                    button.onClick.AddListener(() =>
                    {
                        Choose(index);
                    });
                }
                else
                {
                    // visually indicate disabled state
                    buttonText.color = Color.gray;
                }
            }
        }

        public void Choose(int index)
        {
            if (currentNode.Choices.Count <= index || index < 0)
            {
                return;
            }

            var choice = currentNode.Choices[index];

            if (!IsChoiceAvailable(choice))
            {
                return;
            }

            foreach (var action in choice.Actions)
            {
                action.Execute(initiator, target);
            }
            
            currentNode = choice.NextNode;

            if (currentNode == null)
            {
                EndDialog();
            }
            else
            {
                ShowDialog(currentNode);
            }
        }

        private bool IsChoiceAvailable(DialogChoice choice)
        {
            foreach (var action in choice.Actions)
            {
                if (!action.CanExecute(initiator, target))
                {
                    return false;
                }
            }

            return true;
        }

        private void StartTyping(string line)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            fullLine = line;
            dialogText.text = string.Empty;

            typingCoroutine = StartCoroutine(TypeLine());
        }

        private IEnumerator TypeLine()
        {
            isTyping = true;

            foreach (char c in fullLine)
            {
                dialogText.text += c;
                yield return new WaitForSeconds(typingDelay);
            }

            isTyping = false;
            ShowChoicesIfNeeded();
        }

        private void CompleteLineInstantly()
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            dialogText.text = fullLine;
            isTyping = false;
            ShowChoicesIfNeeded();
        }

        private void ShowChoicesIfNeeded()
        {
            if (currentNode.Choices == null || 
                currentNode.Choices.Count == 0 || 
                currentNode.Choices.Count == 1)
            {
                return;
            }

            CreateChoiceButtons(currentNode);
        }
    }
}