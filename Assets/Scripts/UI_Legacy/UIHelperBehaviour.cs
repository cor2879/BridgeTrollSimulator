/**************************************************
 *  UIHelperBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System.Collections;

    using UnityEngine;
    using UnityEngine.Events;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Exceptions;

    /// <summary>
    /// Defines additional base behaviours for UI Elements
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class UIHelperBehaviour : MonoBehaviour
    {
        public UnityEvent OnEnabled { get; set; } = new UnityEvent();

        public UnityEvent OnDisabled { get; set; } = new UnityEvent();

        public bool IsEnabled { get; private set; }

        /// <summary>
        /// Enables this instance.
        /// </summary>
        public virtual void Enable()
        {
            this.gameObject.SetActive(true);
            this.IsEnabled = true;
        }

        /// <summary>
        /// Disables this instance.
        /// </summary>
        public virtual void Disable()
        {
            this.gameObject.SetActive(false);
            this.IsEnabled = false;
        }

        private void OnEnable()
        {
            if (this.OnEnabled != null)
            {
                this.OnEnabled.Invoke();
            }
        }

        public IEnumerator WaitForDurationThenDoAction(WaitDuration waitDuration)
        {
            while (waitDuration.Duration >= float.Epsilon)
            {
                waitDuration.Duration -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            waitDuration.DoAction.Invoke();
        }

        /// <summary>
        /// Waits for predicate to be false then does the action.
        /// </summary>
        /// <param name="waitAction">The wait action.</param>
        /// <returns></returns>
        public IEnumerator WaitForPredicateToBeFalseThenDoAction(WaitAction waitAction)
        {
            while (waitAction.Predicate())
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            waitAction.DoAction.Invoke();
        }

        public static void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName, string typeName)
        {
            if (parameter == null)
            {
                throw new UIException($"The parameter {parameterName} needs to be set in the Unity Edtior for the {typeName}.");
            }
        }
    }
}
