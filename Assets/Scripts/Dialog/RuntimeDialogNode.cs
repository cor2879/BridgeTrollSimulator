using System.Collections.Generic;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Actions;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Policies;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog
{
    public class RuntimeDialogNode
    {
        public string Text;
        public List<GeneratedOption> Options = new();
        public EntityController Initiator;
        public EntityController Target;
    }
}