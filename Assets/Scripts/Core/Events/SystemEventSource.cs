using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public sealed class SystemEventSource : IEventSource
    {
        public string SourceName { get; }

        public GameSystemType SystemType => GameSystemType.System;

        public SystemEventSource(string name)
        {
            SourceName = name;
        }
    }
}