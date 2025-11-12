#pragma warning disable CS0649

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using UnityEngine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Interfaces;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Exceptions;

    public class PoolablePrefabLibrary : MonoBehaviour
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static PoolablePrefabLibrary Instance { get; private set; }

        /// <summary>
        /// Executes when this instance is awakened.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
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
