using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterSkills;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Abilities
{
    [CreateAssetMenu(menuName = "BridgeTroll/Social/Abilities/Concede")]
    public class ConcedeSocialAbility : SocialAbility
    {
        public override bool TryExecuteSpecial(
            EntityController initiator,
            EntityController target)
        {
            SocialDuelSystem.Instance.UI.EnableInput(false);

            ModalUISystem.Instance.ShowConfirmationDialog(
                "Really concede this social duel?",
                onYes: () =>
                {
                    initiator.ConcedeSocialDuel(target);
                },
                onNo: () =>
                {
                    SocialDuelSystem.Instance.UI.EnableInput(true);
                });

            return true;
        }
    }
}