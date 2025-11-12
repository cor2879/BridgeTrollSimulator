/**************************************************
 *  GrowBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using UnityEngine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;

    /// <summary>
    /// Defines a behaviour that can make a game object grow over time.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class GrowBehaviour : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private float duration;

        [SerializeField, ReadOnly]
        private Vector2 growthVector;

        public void Grow(Vector2 toScale, float duration)
        {
            var startingVector = new Vector2(this.transform.localScale.x, this.transform.localScale.y);
            var difVector = toScale - startingVector;
            this.growthVector = difVector * (Time.fixedDeltaTime / duration);
            this.duration = duration;
        }

        public void StopGrowing()
        {
            this.duration = 0;
            this.growthVector = Vector2.zero;
        }

        public void FixedUpdate()
        {
            if (this.duration > 0)
            {
                this.transform.localScale = new Vector3(
                    this.transform.localScale.x + this.growthVector.x,
                    this.transform.localScale.y + this.growthVector.y,
                    this.transform.localScale.z);

                this.duration -= Time.fixedDeltaTime;
            }
            else
            {
                this.growthVector = Vector2.zero;
            }
        }
    }
}
