using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities
{
    public static class Dice
    {
        public static int RollD20()
        {
            return Random.Range(1, 21);
        }

        public static int RollDice(DiceType diceType, int count)
        {
            if (diceType != DiceType.None)
            {
                var total = 0;
                var ceiling = (int)diceType;

                for (var i = 0; i < count; i++)
                {
                    total += Random.Range(1, ceiling);
                }
            }
        
            return 0;
        }
    }
}