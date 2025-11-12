namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using UnityEngine;

    using System;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Interfaces;

    [RequireComponent(typeof(IGameManager))]
    public class TitleSceneManager : MonoBehaviour, ISceneManager
    {
        [SerializeField, ReadOnly]
        private IGameManager gameManager;

        public IGameManager GameManager
        {
            get
            {
                if ( this.gameManager is null)
                {
                    this.gameManager = this.GetComponent<IGameManager>();
                }

                return this.gameManager;
            }
        }
    }
}