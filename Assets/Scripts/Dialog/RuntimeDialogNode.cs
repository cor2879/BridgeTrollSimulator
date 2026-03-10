using System.Collections.Generic;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Actions;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Policies;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog
{
    public class RuntimeDialogNode : IDialogRenderable
    {
        public string Text { get; set; }
        public List<GeneratedOption> Options { get; set; } = new();
        public EntityController Initiator;
        public EntityController Target;
    }
}