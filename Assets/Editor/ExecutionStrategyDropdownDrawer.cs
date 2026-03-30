#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Strategies;

[CustomPropertyDrawer(typeof(ExecutionStrategyDropdownAttribute))]
public class ExecutionStrategyDropdownDrawer : PropertyDrawer
{
    private Type[] strategyTypes;

    private void Init()
    {
        if (strategyTypes != null)
            return;

        strategyTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                typeof(AbilityExecutionStrategy).IsAssignableFrom(t) &&
                !t.IsAbstract)
            .ToArray();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Init();

        string current = property.stringValue;

        int index = Array.FindIndex(strategyTypes, t => t.FullName == current);
        if (index < 0) index = 0;

        string[] options = strategyTypes.Select(t => t.Name).ToArray();

        int newIndex = EditorGUI.Popup(position, label.text, index, options);

        property.stringValue = strategyTypes[newIndex].FullName;
    }
}
#endif