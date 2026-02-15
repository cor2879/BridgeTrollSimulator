/**************************************************
 *  SelectableBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;

    /// <summary>
    /// Adds standard behaviour to selectable UI elements
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    /// <seealso cref="UnityEngine.EventSystems.ISelectHandler" />
    [RequireComponent(typeof(Selectable))]
    public class SelectableBehaviour : MonoBehaviour, ISelectHandler
    {
        /// <summary>
        /// Called when the element is selected.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        public void OnSelect(BaseEventData eventData)
        {
            /* var soundEffectManager = GameManager.Instance != null ?
                GameManager.Instance.SoundEffectManager :
                null; */

            // soundEffectManager?.PlayAudioOnce(SoundClips.Click1);
        }
    }
}
