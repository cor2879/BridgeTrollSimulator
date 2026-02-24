using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class GoldFeedbackSystem : MonoBehaviour
    {
        private void OnEnable()
        {
            GameEventBus.Subscribe<GoldFeedbackEvent>(OnGoldFeedback);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<GoldFeedbackEvent>(OnGoldFeedback);
        }

        private void OnGoldFeedback(GoldFeedbackEvent evt)
        {
            SpawnPopup(evt);
        }

        private void SpawnPopup(GoldFeedbackEvent evt)
        {
            var popupUI = evt.Target.GoldPopupUI;

            if (popupUI == null)
            {
                return;
            }

            popupUI.Play(evt.PreviousAmount, evt.NewAmount, evt.Delta);
        }
    }
}