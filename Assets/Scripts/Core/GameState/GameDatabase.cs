using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameState
{
    public class GameDatabase : MonoBehaviour
    {
        private static GameDatabase _instance;

        public static GameDatabase Instance 
        { 
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<GameDatabase>();
                }

                return _instance;
            }
        }

        [SerializeField]
        private DispositionMatrix dispositionMatrix;

        [SerializeField]
        private EntityController player;

        public DispositionMatrix Dispositions => dispositionMatrix;
        public EntityController Player => player;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            if (player == null)
            {
                Debug.LogError("GameDatabase: Player reference not assigned.");
            }
        }
    }
}