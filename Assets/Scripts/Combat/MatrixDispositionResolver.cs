using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Combat
{
    public class MatrixDispositionResolver : IFactionDispositionResolver
    {
        private readonly DispositionMatrix matrix;

        public MatrixDispositionResolver(DispositionMatrix matrix)
        {
            this.matrix = matrix;
        }

        public bool IsHostile(CombatFaction a, CombatFaction b)
        {
            return matrix.IsHostile(a, b);
        }
    }
}