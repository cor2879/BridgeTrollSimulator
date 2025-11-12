/**************************************************
 *  DataFieldDefinition.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using UnityEngine;

    public class DataFieldDefinition
    {
        [SerializeField]
        private string displayName;

        [SerializeField]
        private string propertyName;

        public string DisplayName { get => this.displayName; set => this.displayName = value; }

        public string PropertyName { get => this.propertyName; set => this.propertyName = value; }
    }
}
