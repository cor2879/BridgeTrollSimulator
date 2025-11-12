/**************************************************
 *  SpawnerBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using UnityEngine;

    /// <summary>
    /// Defines the behaviours for a GameObject that can spawn other GameObjects.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class SpawnerBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Spawns an object from the specified prefab.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns></returns>
        public GameObject SpawnObject(GameObject prefab)
        {
            if (prefab != null)
            {
                return Instantiate(prefab, GetNewObjectPosition(transform.position), Quaternion.identity);
            }

            return null;
        }

        /// <summary>
        /// Gets the new object position.
        /// </summary>
        /// <param name="spawnerPosition">The spawner position.</param>
        /// <returns></returns>
        private static Vector3 GetNewObjectPosition(Vector3 spawnerPosition)
        {
            return new Vector3(spawnerPosition.x, spawnerPosition.y, 0);
        }
    }
}
