using UnityEngine;
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

        private DialogSequence currentSequence;
        private int currentIndex;
        private EntityController initiator;
        private EntityController target;

        private void OnEnable()
        {
            GameEventBus.Subscribe<DialogStartedEvent>(OnDialogStarted);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<DialogStartedEvent>(OnDialogStarted);
        }

        private void OnDialogStarted(DialogStartedEvent evt)
        {
            Debug.Log(evt.ToString());

            if (evt.Sequence is null)
            {
                Debug.LogWarning("DialogStartedEvent received with Null Sequence");
            }

            currentSequence = evt.Sequence;
            initiator = evt.Initiator;
            target = evt.Target;
            currentIndex = 0;

            ShowDialog(evt.Initiator.SourceName);
        }

        private void Update()
        {
            if (currentSequence is null) 
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentIndex++;

                if (currentIndex >= currentSequence.Lines.Count)
                {
                    EndDialog();
                }
                else
                {
                    ShowLine();
                }
            }
        }

        public void ShowDialog(string speaker)
        {
            panel.SetActive(true);
            speakerNameText.text = speaker;
            ShowLine();
        }

        private void ShowLine()
        {
            dialogText.text = currentSequence.Lines[currentIndex].Text;
        }

        private void EndDialog()
        {
            panel.SetActive(false);

            GameEventBus.Publish(
                new DialogEndedEvent(initiator, target, Time.frameCount));

            currentSequence = null;
        }
    }
}