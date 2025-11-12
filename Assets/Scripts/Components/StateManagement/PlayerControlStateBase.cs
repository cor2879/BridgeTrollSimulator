/**************************************************
 *  PlayerStateBase.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components.StateManagement
{
    using System;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;

    public class PlayerControlStateBase : EntityControlStateBase<PlayerBehaviour>
    {
        private static PlayerControlStateBase instance;

        public static PlayerControlStateBase Instance { get => instance; private set => instance = value; }

        protected PlayerControlStateBase(PlayerBehaviour player) : base(player)
        { }

        public static PlayerControlStateBase GetInstance(PlayerBehaviour player)
        {
            if (Instance == null)
            {
                Instance = new PlayerControlStateBase(player);
            }

            return Instance;
        }
    }
}
