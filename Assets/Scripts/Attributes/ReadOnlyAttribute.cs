/**************************************************
 *  ReadOnlyAttribute.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Defines an attribute for Monobehaviour fields that allows them to be viewed in the
    /// Unity Editor but not modified.
    /// </summary>
    /// <seealso cref="UnityEngine.PropertyAttribute" />
    public class ReadOnlyAttribute
#if UNITY_EDITOR        
        : PropertyAttribute
#else
        : System.Attribute
#endif

    {
    }

#if UNITY_EDITOR
    /// <summary>
    /// Defines the Drawer used by the UnityEditor when handling the ReadOnlyAttribute.
    /// </summary>
    /// <seealso cref="UnityEditor.PropertyDrawer" />
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : UnityEditor.PropertyDrawer
    {
        /// <summary>
        /// Override this method to make your own IMGUI based GUI for the property.
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the property GUI.</param>
        /// <param name="property">The SerializedProperty to make the custom GUI for.</param>
        /// <param name="label">The label of this property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
#endif
}
