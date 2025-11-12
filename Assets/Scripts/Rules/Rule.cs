/**************************************************
 *  Rule.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Rules
{
    using System;

    public abstract class Rule
    {
        private Rule() { }

        public Rule(string name)
        {
            this.Name = name;
        }

        public abstract bool Evaluate();

        public Action OnUpdate;

        public string Name { get; set; }

        protected void UpdateListeners()
        {
            this.OnUpdate?.Invoke();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
