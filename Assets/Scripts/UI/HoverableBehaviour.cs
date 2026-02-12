/**************************************************
 *  HoverableBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;

    /// <summary>
    /// Adds standard behaviour to hoverable UI elements
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    /// <seealso cref="UnityEngine.EventSystems.IPointerEnterHandler" />
    public class HoverableBehaviour : MonoBehaviour, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            // GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.Blip);
        }
    }
}
