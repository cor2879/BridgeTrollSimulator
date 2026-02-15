#pragma warning disable CS0649
/**************************************************
 *  OnSelectScrollBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;

    [RequireComponent(typeof(Scrollbar))]
    public class OnSelectScrollBehaviour : MonoBehaviour
    {
        private Scrollbar scrollbar;
        private float[] rowStops;

        public float StepHeight { get; set; }

        public float ViewPortHeight { get; set; }

        public int RowCount { get; set; }

        private float StepValue
        {
            get
            {
                var divisor = this.TotalHeight - ViewPortHeight;

                if (Math.Abs(divisor) < float.Epsilon)
                {
                    return 1.0f;
                }

                return (this.StepHeight / (this.TotalHeight - ViewPortHeight));
            }
        }

        private float TotalHeight
        {
            get
            {
                return this.RowCount * StepHeight;
            }
        }

        private float[] RowStops
        {
            get
            {
                if (this.rowStops == null || this.RowCount != this.rowStops.Length)
                {
                    this.rowStops = new float[this.RowCount];

                    if (this.RowCount > 0)
                    {
                        this.rowStops[0] = 1.0f;

                        for (var i = 1; i < this.RowCount; i++)
                        {
                            this.rowStops[i] = this.rowStops[i - 1] - this.StepValue;
                        }
                    }
                }

                return this.rowStops;
            }
        }

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

        public void ScrollToView(int row)
        {
            if (this.RowStops.Length > row && row > -1)
            {
                this.Scrollbar.value = (this.RowStops[row]);
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
    }
}
