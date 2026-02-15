using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces
{
    public interface IEncounterable
    {
        GameObject GameObject { get; }

        void HandleEncounter(IEncounterable other);
    }
}