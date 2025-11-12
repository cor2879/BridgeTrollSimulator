/**************************************************
 *  UIWindowBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System.Collections;

    using UnityEngine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;

    /// <summary>
    /// Defines base behaviours for UI Windows
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.UIHelperBehaviour" />
    public class UIWindowBehaviour : UIHelperBehaviour
    {
        /// <summary>
        /// Updates this instance when the Unity Engine updates each frame.
        /// </summary>
        public virtual void Update()
        {
            if (InputExtension.IsCloseWindowPressed())
            {
                StartCoroutine(nameof(this.WaitForButtonUpThenClose));
            }
        }

        /// <summary>
        /// Determines whether this instance is active.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </returns>
        public bool IsActive()
        {
            return this.gameObject.activeInHierarchy;
        }

        /// <summary>
        /// Called when this instance is enabled.
        /// </summary>
        public virtual void OnEnable()
        {
            GameManager.Instance.PauseAction = true;
        }

        /// <summary>
        /// Called when  this instance is disabled.
        /// </summary>
        public virtual void OnDisable()
        {
            GameManager.Instance.PauseAction = false;
        }

        /// <summary>
        /// Waits for the CloseWindow button to be released, then closes.
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitForButtonUpThenClose()
        {
            while (InputExtension.IsCloseWindowPressed())
            {
                yield return new WaitForFixedUpdate();
            }

            this.Disable();
        }
    }
}
