using UnityEngine;
using System.Collections.Generic;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Policies
{
    public abstract class OptionPolicy : ScriptableObject, IOptionPolicy
    {
        public abstract List<GeneratedOption> GetAvailableOptions(EntityController initiator, EntityController target);
    }
}