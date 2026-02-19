using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Enums;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Interfaces
{
    public interface IFactionDispositionResolver
    {
        bool IsHostile(CombatFaction a, CombatFaction b);
    }
}