/**************************************************
 *  Statistic.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using System;
    using UnityEngine;

    public abstract class Statistic
    {
        public static Statistic<int> MuglumpsKilled = new IntegerStatistic()
        {
            Name = "MuglumpsKilled",
            DisplayName = "Muglumps Killed",
        };

        public static Statistic<int> BlackMuglumpsKilled = new IntegerStatistic()
        {
            Name = "BlackMuglumpsKilled",
            DisplayName = "Black Muglumps Killed",
        };

        public static Statistic<int> BlueMuglumpsKilled = new IntegerStatistic()
        {
            Name = "BlueMuglumpsKilled",
            DisplayName = "Blue Muglumps Killed"
        };

        public static Statistic<int> GoldMuglumpsKilled = new IntegerStatistic()
        {
            Name = "GoldMuglumpsKilled",
            DisplayName = "Gold Muglumps Killed"
        };

        public static Statistic<int> SilverbackMuglumpsKilled = new IntegerStatistic()
        {
            Name = "SilverbackMuglumpsKilled",
            DisplayName = "Silverback Muglumps Killed"
        };

        public static Statistic<int> SpecialKills = new IntegerStatistic()
        {
            Name = "SpecialKills",
            DisplayName = "Special Kills"
        };

        public static Statistic<int> BatsRidden = new IntegerStatistic()
        {
            Name = "BatsRidden",
            DisplayName = "Bats Ridden"
        };

        public static Statistic<int> RoomsExplored = new IntegerStatistic()
        {
            Name = "RoomsExplored",
            DisplayName = "Rooms Explored"
        };

        public static Statistic<int> ArrowsFired = new IntegerStatistic()
        {
            Name = "ArrowsFired",
            DisplayName = "Arrows Fired"
        };

        public static Statistic<int> ArrowsHit = new IntegerStatistic()
        {
            Name = "ArrowsHit",
            DisplayName = "Arrows Hit"
        };

        public static Statistic<int> BatsFlashed = new IntegerStatistic()
        {
            Name = "BatsFlashed",
            DisplayName = "Bats chased away with a flash arrow"
        };

        public static Statistic<int> CrownsFound = new IntegerStatistic()
        {
            Name = "CrownsFound",
            DisplayName = "Crowns found"
        };

        public static Statistic<int> BearTrapsSet = new IntegerStatistic()
        {
            Name = "BearTrapsSet",
            DisplayName = "Bear Traps Set"
        };

        public static Statistic<int> MuglumpsTrapped = new IntegerStatistic()
        {
            Name = "MuglumpsTrapped",
            DisplayName = "Muglumps Trapped"
        };

        public static Statistic<int> Overkills = new IntegerStatistic()
        {
            Name = "Overkills",
            DisplayName = "Overkills"
        };

        protected Statistic() { }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public Action OnUpdate;
    }

    public abstract class Statistic<TValue> : Statistic where TValue : struct, IComparable<TValue>
    {
        protected PlayerPrefsProperty<TValue> value;

        public abstract TValue Value { get; set; }
    }

    public class IntegerStatistic : Statistic<int>
    {
        public override int Value
        {
            get
            {
                if (this.value == null)
                {
                    this.value = new PlayerPrefsIntProperty(this.Name);
                }

                return this.value.Get();
            }

            set
            {
                this.value.Set(value);
                this.OnUpdate?.Invoke();
            }
        }
    }
}
