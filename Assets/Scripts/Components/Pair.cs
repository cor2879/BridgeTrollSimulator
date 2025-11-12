/**************************************************
 *  Pair.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    public class Pair<TFirst, TSecond>
    {
        public Pair() { }

        public Pair(TFirst first, TSecond second)
        {
            this.First = first;
            this.Second = second;
        }

        public TFirst First { get; set; }

        public TSecond Second { get; set; }

    }
}
