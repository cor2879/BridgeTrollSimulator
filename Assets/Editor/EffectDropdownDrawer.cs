#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects;

[CustomPropertyDrawer(typeof(EffectDropdownAttribute))]
public class EffectDropdownDrawer : PropertyDrawer
{
    private Type[] effectTypes;

    private void Init()
    {
        if (effectTypes != null) return;

        effectTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                typeof(StatusEffect).IsAssignableFrom(t) &&
                !t.IsAbstract)
            .ToArray();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Init();

        string current = property.stringValue;

        int index = Array.FindIndex(effectTypes, t => t.FullName == current);
        if (index < 0) index = 0;

        string[] options = effectTypes.Select(t => t.Name).ToArray();

        int newIndex = EditorGUI.Popup(position, label.text, index, options);

        property.stringValue = effectTypes[newIndex].FullName;
    }
}
#endif