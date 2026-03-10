using System.Collections.Generic;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Policies;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Interfaces
{
    public interface IDialogRenderable
    {
        string Text { get; }
        List<GeneratedOption> Options { get; }
    }
}