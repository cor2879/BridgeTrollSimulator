using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Actions;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Policies
{
    [CreateAssetMenu(menuName = "BridgeTroll/DialogActions/Generate Options")]
    public class GenerateOptionsAction : DialogAction
    {   
        [SerializeField]
        private OptionPolicy policy;

        public override void Execute(EntityController initiator, EntityController target)
        {
            var options = policy.GetAvailableOptions(initiator, target);

            DialogSystem.Instance.ShowGeneratedOptions(options);
        }
    }
}