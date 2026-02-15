#pragma warning disable CS0649
/**************************************************
 *  BadgeEarnedPanelBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Exception;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;

    /// <summary>
    /// Defines a behaviour for the BadgePanel UI.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(ExpandUpBehaviour))]
    public class BadgeEarnedPanelBehaviour : UIHelperBehaviour
    {
        private const float height = 120.0f;

        [SerializeField]
        private TextLabelBehaviour badgeEarnedTextLabel;

        [SerializeField]
        private RawImage badgeEarnedRawImage;

        [SerializeField, ReadOnly]
        private ExpandUpBehaviour expandUpBehaviour;

        public ExpandUpBehaviour ExpandUpBehaviour
        {
            get
            {
                if (this.expandUpBehaviour == null)
                {
                    this.expandUpBehaviour = this.GetComponent<ExpandUpBehaviour>();
                }

                return this.expandUpBehaviour;
            }
        }

        public Texture Texture
        {
            get => this.badgeEarnedRawImage.texture;
            private set => this.badgeEarnedRawImage.texture = value;
        }

        public string Text
        {
            get => this.badgeEarnedTextLabel.Text;
            set => this.badgeEarnedTextLabel.Text = value;
        }

        public void Show(Badge badge, float duration, Action onShowBadgeComplete)
        {
            if (badge.Earned)
            {
                this.ExpandUpBehaviour.ExpandedHeight = BadgeEarnedPanelBehaviour.height;
                this.Texture = Resources.Load<Texture2D>(badge.TextureName);
                this.Text = badge.DisplayName;
                // GameManager.Instance.MusicManager.PlayAudioOnce(SoundClips.BadgeEarned);
                this.Enable();
                StartCoroutine(
                    nameof(this.ShowForDuration),
                    new ShowForDurationOptions()
                    {
                        Duration = duration,
                        OnComplete = onShowBadgeComplete
                    });
            }
            else
            {
                throw new BadgeException(badge, $"{badge.Name} badge not earned!");
            }
        }

        public void Start()
        {
            this.DisableContent();

            this.ExpandUpBehaviour.OnExpanded = () =>
            {
                this.EnableContent();
            };
        }

        private IEnumerator ShowForDuration(ShowForDurationOptions options)
        {
            while (options.Duration > float.Epsilon)
            {
                options.Duration -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            this.Disable();

            options.OnComplete?.Invoke();
        }

        public void OnDisable()
        {
            this.DisableContent();
        }

        private void DisableContent()
        {
            this.badgeEarnedTextLabel.Disable();
            this.badgeEarnedRawImage.GetComponent<UIHelperBehaviour>().Disable();
        }

        private void EnableContent()
        {
            this.badgeEarnedTextLabel.Enable();
            this.badgeEarnedRawImage.GetComponent<UIHelperBehaviour>().Enable();
        }

        public override void Disable()
        {
            base.Disable();
        }

        private class ShowForDurationOptions
        {
            public float Duration { get; set; }

            public Action OnComplete { get; set; }
        }
    }
}
