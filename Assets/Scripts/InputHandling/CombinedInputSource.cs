<<<<<<< HEAD
using UnityEngine;
=======
sing UnityEngine;
>>>>>>> origin/main

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.InputHandling
{
    public class CombinedInputSource : IInputSource
    {
        private readonly IInputSource primary;
        private readonly IInputSource secondary;

<<<<<<< HEAD
        public CombinedInputSource(IInputSource primary, IInputSource secondary)
=======
        public CombinedInputSource(
            IInputSource primary,
            IInputSource secondary)
>>>>>>> origin/main
        {
            this.primary = primary;
            this.secondary = secondary;
        }

        public float GetHorizontal()
        {
            float secondaryValue = secondary?.GetHorizontal() ?? 0f;

            if (!Mathf.Approximately(secondaryValue, 0f))
            {
                return secondaryValue;
            }

            return primary?.GetHorizontal() ?? 0f;
        }

        public float GetVertical()
        {
            float secondaryValue = secondary?.GetVertical() ?? 0f;

            if (!Mathf.Approximately(secondaryValue, 0f))
            {
                return secondaryValue;
            }

            return primary?.GetVertical() ?? 0f;
        }

        public bool AttackPressed()
        {
            return (primary?.AttackPressed() ?? false) ||
                   (secondary?.AttackPressed() ?? false);
        }

        public bool ThreatenPressed()
        {
            return (primary?.ThreatenPressed() ?? false) ||
                   (secondary?.ThreatenPressed() ?? false);
        }

        public bool ItchPressed()
        {
            return (primary?.ItchPressed() ?? false) ||
                   (secondary?.ItchPressed() ?? false);
        }

        public bool JumpPressed()
        {
            return (primary?.JumpPressed() ?? false) ||
                   (secondary?.JumpPressed() ?? false);
        }

        public bool DiePressed()
        {
            return (primary?.DiePressed() ?? false) ||
                   (secondary?.DiePressed() ?? false);
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> origin/main
