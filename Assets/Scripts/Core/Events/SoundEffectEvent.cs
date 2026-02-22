using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class SoundEffectEvent : GameEvent, ISoundEffectEvent
    {
        public AudioClip Clip { get; private set; }
        
        public SoundEffectEvent(
            IEventSource initiator,
            AudioClip clip,
            int frame)
            : base(initiator, frame)
        {
            this.Clip = clip;
        }
    }
}