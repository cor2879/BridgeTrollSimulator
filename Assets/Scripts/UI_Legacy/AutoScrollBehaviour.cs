#pragma warning disable CS0649
/**************************************************
 *  AutoScrollBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Defines a behaviour for Scrollbar UI which causes it to automatically scroll to the bottom when
    /// the scrollable area is expanded.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(Scrollbar))]
    public class AutoScrollBehaviour : MonoBehaviour
    {
        private const float Tolerance = 0.2f;

        private Scrollbar scrollbar;
        private float lastSize;

        [SerializeField]
        private ScrollBehaviour scrollBehaviour;

        /// <summary>
        /// Gets the scrollbar.
        /// </summary>
        /// <value>
        /// The scrollbar.
        /// </value>
        private Scrollbar Scrollbar
        {
            get
            {
                if (this.scrollbar == null)
                {
                    this.scrollbar = this.GetComponent<Scrollbar>();
                }

                return this.scrollbar;
            }
        }

        /// <summary>
        /// Executed during the Update event of the object lifecycle.
        /// </summary>
        public void Update()
        {
            var size = this.Scrollbar.size;
            var difference = this.lastSize - size;

            if (Mathf.Abs(difference) > Tolerance && size < 1.0f)
            {
                AutoScroll();
            }

            this.lastSize = size;
        }

        private void AutoScroll()
        {
            switch (this.scrollBehaviour)
            {
                case ScrollBehaviour.ScrollToTop:
                    this.ScrollToTop();
                    break;
                case ScrollBehaviour.ScrollToBottom:
                    this.ScrollToBottom();
                    break;
            }
        }

        /// <summary>
        /// Scrolls to the bottom.
        /// </summary>
        private void ScrollToBottom()
        {
            this.Scrollbar.value = 0;
        }

        private void ScrollToTop()
        {
            this.Scrollbar.value = 1.0f;
        }

        public void OnBecameVisible()
        {
            this.AutoScroll();
        }

        public enum ScrollBehaviour
        {
            ScrollToTop,
            ScrollToBottom
        };
    }
}
