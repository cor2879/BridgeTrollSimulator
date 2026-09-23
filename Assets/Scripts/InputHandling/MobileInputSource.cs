using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.InputHandling
{
    public class MobileInputSource : MonoBehaviour, IInputSource
    {
        private float horizontal;

        public float GetHorizontal()
        {
            return horizontal;
        }

        public float GetVertical()
        {
            return 0f;
        }

        public void SetHorizontal(float value)
        {
            horizontal = Mathf.Clamp(value, -1f, 1f);
        }

        public void ReleaseHorizontal(float value)
        {
            if (Mathf.Approximately(horizontal, value))
            {
                horizontal = 0f;
            }
        }

        private void OnDisable()
        {
            horizontal = 0f;
        }

        public bool AttackPressed() => false;
        public bool ThreatenPressed() => false;
        public bool ItchPressed() => false;
        public bool JumpPressed() => false;
        public bool DiePressed() => false;
    }
}