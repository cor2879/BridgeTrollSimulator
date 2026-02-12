using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
using OldSchoolGames.BridgeTrollSimulator.Scripts.InputHandling;
using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities
{
    public class TrollController : EntityController
    {
        protected override void Awake()
        {
            base.Awake();
            this.inputSource = new KeyboardInputSource();
        }

        protected override void ProcessInput()
        {
            base.ProcessInput();

            if (inputSource is null)
            {
                return;
            }

            if (inputSource.AttackPressed())
            {
                TriggerAction(Constants.Triggers.Attack);
            }

            if (inputSource.ThreatenPressed())
            {
                TriggerAction(Constants.Triggers.Threaten);
            }

            if (inputSource.ItchPressed())
            {
                TriggerAction(Constants.Triggers.Itch);
            }

            if (inputSource.JumpPressed())
            {
                TriggerAction(Constants.Triggers.Jump);
            }

            if (inputSource.DiePressed())
            {
                TriggerAction(Constants.Triggers.Die);
            }
        }
    }
}