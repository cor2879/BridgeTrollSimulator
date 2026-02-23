using System.Collections.Generic;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Policies
{
    public interface IOptionPolicy
    {
        List<GeneratedOption> GetAvailableOptions(
            EntityController initiator,
            EntityController target);
    }
}