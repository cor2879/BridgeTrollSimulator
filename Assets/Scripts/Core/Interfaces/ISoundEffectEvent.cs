using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces
{
    public interface ISoundEffectEvent : IGameEvent
    {
        AudioClip Clip { get; }
    }
}