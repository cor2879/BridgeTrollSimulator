#if UNITY_EDITOR

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;

    [CustomEditor(typeof(DataFieldDefinitionList))]
    public class DataFieldDefinitionListInspector
        : Editor
    {
        private DisplayFieldType displayFieldType;
        private DataFieldDefinitionList dataFieldDefinitionList;
        private int listSize;

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("displayName"), true);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("propertyName"), true);
            this.serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif