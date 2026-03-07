using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities
{
    public static class Dice
    {
        public static int RollD20()
        {
            return Random.Range(1, 21);
        }
    }
}