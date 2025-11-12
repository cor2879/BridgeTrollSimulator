#pragma warning disable CS0649
/**************************************************
 *  OldSchoolGamesPanelBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System.Collections;

    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Defines the behaviours for the OldSchoolGames Logo UI Panel
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class OldSchoolGamesPanelBehaviour : ExitEarlyBehaviour
    {
        /// <summary>
        /// The Old School Games text field
        /// </summary>
        [SerializeField]
        private Text OldSchoolGamesTextField;

        /// <summary>
        /// The presents text field
        /// </summary>
        [SerializeField]
        private Text presentsTextField;

        /// <summary>
        /// The fade time
        /// </summary>
        [SerializeField]
        private float fadeTime;

        /// <summary>
        /// The wait time
        /// </summary>
        [SerializeField]
        private float waitTime;

        /// <summary>
        /// Executes during the Start event of the GameObject life cycle
        /// </summary>
        public void Start()
        {
            this.OldSchoolGamesTextField.canvasRenderer.SetAlpha(0.0f);
            this.presentsTextField.canvasRenderer.SetAlpha(0.0f);

            this.OldSchoolGamesTextField.CrossFadeAlpha(1.0f, this.fadeTime, false);

            // StartCoroutine(nameof(this.OldSchoolGamesIntroLogoStep1));
        }

        /// <summary>
        /// Old School Games intro logo step1.
        /// </summary>
        /// <returns></returns>
        private IEnumerator OldSchoolGamesIntroLogoStep1()
        {
            yield return new WaitForSeconds(this.fadeTime + waitTime);

            this.OldSchoolGamesTextField.CrossFadeAlpha(0.0f, fadeTime, false);

            // StartCoroutine(nameof(this.OldSchoolGamesIntroLogoStep2));
        }

        /// <summary>
        /// Old School Games intro logo step2.
        /// </summary>
        /// <returns></returns>
        private IEnumerator OldSchoolGamesIntroLogoStep2()
        {
            yield return new WaitForSeconds(this.fadeTime);

            this.presentsTextField.CrossFadeAlpha(1.0f, fadeTime, false);

            // StartCoroutine(nameof(this.OldSchoolGamesIntroLogoStep3));
        }

        /// <summary>
        /// Old School Games intro logo step3.
        /// </summary>
        /// <returns></returns>
        private IEnumerator OldSchoolGamesIntroLogoStep3()
        {
            yield return new WaitForSeconds(this.fadeTime + this.waitTime);

            this.presentsTextField.CrossFadeAlpha(0.0f, fadeTime, false);

            // StartCoroutine(nameof(this.OldSchoolGamesIntroLogoStep4));
        }


        protected override void Exit()
        {
            base.Exit();
        }
    }
}
