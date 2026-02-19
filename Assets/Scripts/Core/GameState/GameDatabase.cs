using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameState
{
    public class GameDatabase : MonoBehaviour
    {
        public static GameDatabase Instance { get; private set; }

        [SerializeField]
        private DispositionMatrix dispositionMatrix;

        [SerializeField]
        private EntityController player;

        public DispositionMatrix Dispositions => dispositionMatrix;
        public EntityController Player => player;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (player == null)
            {
                Debug.LogError("GameDatabase: Player reference not assigned.");
            }
        }
    }
}