/**************************************************
 *  TextLabelBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Defines the behaviour for UI Text Elements that are specifically used to label other UI elements.
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.UIHelperBehaviour" />
    [RequireComponent(typeof(Text))]
    public class TextLabelBehaviour : UIHelperBehaviour
    {
        /// <summary>
        /// The text component
        /// </summary>
        private Text textComponent;

        /// <summary>
        /// Gets the text component.
        /// </summary>
        /// <value>
        /// The text component.
        /// </value>
        private Text TextComponent
        {
            get
            {
                if (this.textComponent == null)
                {
                    this.textComponent = this.GetComponent<Text>();
                }

                return this.textComponent;
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get => this.TextComponent.text;
            set => this.TextComponent.text = value;
        }
    }
}
