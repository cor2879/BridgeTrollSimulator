#pragma warning disable CS0649
/**************************************************
 *  TextButtonBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{

    using UnityEngine;
    using UnityEngine.UI;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Exceptions;

    public class TextButtonBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Text textBox;

        private Text TextBox
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

        public string Text
        {
            get => this.TextBox.text;
            set => this.TextBox.text = value;
        }
    }
}
