using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.InputHandling
{
    public class KeyboardInputSource : IInputSource
    {
        public float GetHorizontal()
        {
            return Input.GetAxisRaw(Constants.HorizontalAxis);
        }

        public float GetVertical()
        {
            return Input.GetAxisRaw(Constants.VerticalAxis);
        }

        public bool AttackPressed()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }

        public bool ThreatenPressed()
        {
            return Input.GetKeyDown(KeyCode.T);
        }

        public bool ItchPressed()
        {
            return Input.GetKeyDown(KeyCode.I);
        }

        public bool JumpPressed()
        {
            return Input.GetKeyDown(KeyCode.J);
        }

        public bool DiePressed()
        {
            return Input.GetKeyDown(KeyCode.K);
        }
    }
}