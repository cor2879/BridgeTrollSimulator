/**************************************************
 *  EndGameRule.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Rules
{
    using System;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;

    public class EndGameRule : Rule
    {
        private Predicate<GameOverSettings> Evaluator { get; }

        public EndGameRule(string name, Predicate<GameOverSettings> evaluator)
            : base(name)
        {
            this.Evaluator = evaluator;
        }

        public GameOverSettings GameOverSettings { get; set; }

        public void Update()
        {
            this.UpdateListeners();
        }

        public override bool Evaluate()
        {
            if (this.GameOverSettings == null)
            {
                return false;
            }

            return this.Evaluator.Invoke(this.GameOverSettings);
        }
    }
}
