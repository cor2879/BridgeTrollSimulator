using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement
{
    public class GameStateChangedEvent : GameEvent
    {
        public GameState Previous { get; }
        public GameState Current { get; }

        public GameStateChangedEvent(
            IEventSource sender,
            GameState previous,
            GameState current,
            int frame)
            : base(sender, frame)
        {
            Previous = previous;
            Current = current;
        }

        public override string ToString()
        {
            return $"{nameof(GameStateChangedEvent)}: Sender: {Sender.SourceName} Previous: {Previous} Current: {Current} @ Frame {Frame}";
        }
    }
}