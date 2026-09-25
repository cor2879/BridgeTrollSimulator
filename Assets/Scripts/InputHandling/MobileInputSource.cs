using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.InputHandling
{
    public class MobileInputSource : MonoBehaviour, IInputSource
    {
        private bool leftPressed;
        private bool rightPressed;

        public float GetHorizontal()
        {
            if (leftPressed == rightPressed)
            {
                return 0f;
            }

            return leftPressed ? -1f : 1f;
        }

        public float GetVertical()
        {
            return 0f;
        }

        public void PressHorizontal(float direction)
        {
            if (direction < 0f)
            {
                leftPressed = true;
            }
            else if (direction > 0f)
            {
                rightPressed = true;
            }
        }

        public void ReleaseHorizontal(float direction)
        {
            if (direction < 0f)
            {
                leftPressed = false;
            }
            else if (direction > 0f)
            {
                rightPressed = false;
            }
        }

        private void OnDisable()
        {
            leftPressed = false;
            rightPressed = false;
        }

        public bool AttackPressed() => false;
        public bool ThreatenPressed() => false;
        public bool ItchPressed() => false;
        public bool JumpPressed() => false;
        public bool DiePressed() => false;
    }
}
