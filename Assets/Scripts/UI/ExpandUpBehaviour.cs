namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System;
    using System.Collections;

    using UnityEngine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Exceptions;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    [RequireComponent(typeof(RectTransform))]
    public class ExpandUpBehaviour : MonoBehaviour
    {
        private static float expandDuration = 0.5f;

        [SerializeField, ReadOnly]
        private RectTransform rectTransform;

        [SerializeField, ReadOnly]
        private float expandedHeight;

        public float ExpandedHeight
        {
            get => this.expandedHeight;
            set => this.expandedHeight = value;
        }

        public Action OnExpanded { get; set; }

        public RectTransform RectTransform
        {
            get
            {
                if (this.rectTransform == null)
                {
                    this.rectTransform = this.GetComponent<RectTransform>();
                }

                return this.rectTransform;
            }
        }

        public void Start()
        {
            this.ResetHeight();
        }

        public void OnDisable()
        {
            this.ResetHeight();
        }

        public void OnEnable()
        {
            StartCoroutine(nameof(this.ExpandToHeight));
        }

        private void ResetHeight()
        {
            this.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.0f);
        }

        public IEnumerator ExpandToHeight()
        {
            var growthPerCycle = (Time.fixedDeltaTime * this.ExpandedHeight) / expandDuration;
            var duration = expandDuration;

            while (duration > float.Epsilon)
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);

                var newHeight = this.RectTransform.rect.height + growthPerCycle;
                this.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
                duration -= Time.fixedDeltaTime;
            }

            this.OnExpanded?.Invoke();
        }
    }
}
