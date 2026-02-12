/**************************************************
 *  BadgeManager.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;

    public class BadgeManager : MonoBehaviour
    {
        public const float ShowBadgeDuration = 3.0f;

        public Badge[] Badges
        {
            get; private set;
        }

        private Queue<Badge> EarnedBadgeQueue { get; } = new Queue<Badge>();

        private void OnBadgeEarned(Badge badge)
        {
            this.EarnedBadgeQueue.Enqueue(badge);

            if (this.EarnedBadgeQueue.Count == 1)
            {
                /* GameManager.Instance.ShowBadgeEarned(
                    badge,
                    ShowBadgeDuration,
                    this.OnShowBadgeComplete); */
            }
        }

        public void Start()
        {
            var badges = Badge.GetStaticBadges();

            foreach (var badge in badges)
            {
                if (!badge.Earned)
                {
                    badge.OnBadgeEarned += this.OnBadgeEarned;
                }

                badge.StartListening();
            }

            this.Badges = badges;
        }

        public void OnDestroy()
        {
            foreach (var badge in this.Badges)
            {
                badge.StopListening();
            }
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

        private void OnShowBadgeComplete()
        {
            this.EarnedBadgeQueue.Dequeue();

            if (this.EarnedBadgeQueue.Any())
            {
                var badge = this.EarnedBadgeQueue.Peek();

                if (badge != null)
                {
                    /* GameManager.Instance.ShowBadgeEarned(
                        badge,
                        ShowBadgeDuration,
                        this.OnShowBadgeComplete); */
                }
            }
        }
    }
}
