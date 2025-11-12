#pragma warning disable CS0649
/**************************************************
 *  DataFieldBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System.Collections;
    using System.Reflection;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Exceptions;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    public class DataFieldBehaviour
        : UIHelperBehaviour
    {
        [SerializeField]
        private Text textBox;

        public DataRowBehaviour Row { get; set; }

        public Text Textbox
        {
            get
            {
                if (this.textBox == null)
                {
                    throw new UIException($"The parameter {nameof(this.textBox)} needs to be set in the Unity Editor.");
                }

                return this.textBox;
            }
        }
    }
}
