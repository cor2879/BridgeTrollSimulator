/**************************************************
 *  IFindable.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Interfaces
{
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;

    public interface IFindable
    {
        void HandleEncounter(PlayerBehaviour player);
    }
}
