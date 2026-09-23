using UnityEngine;
using UnityEngine.EventSystems;

using OldSchoolGames.BridgeTrollSimulator.Scripts.InputHandling;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class MobileMoveButton :
        MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerExitHandler
    {
        [SerializeField]
        private MobileInputSource inputSource;

        [SerializeField, Range(-1f, 1f)]
        private float direction;

        public void OnPointerDown(PointerEventData eventData)
        {
            inputSource?.SetHorizontal(direction);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            inputSource?.ReleaseHorizontal(direction);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inputSource?.ReleaseHorizontal(direction);
        }

        private void OnDisable()
        {
            inputSource?.ReleaseHorizontal(direction);
        }
    }
}