using System;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Policies
{
    [Serializable]
    public class GeneratedOption
    {
        public string Label;
        public Action<EntityController, EntityController> Execute;
    }
}

