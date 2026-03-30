using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Requirements;

[CustomPropertyDrawer(typeof(RequirementDropdownAttribute))]
public class RequirementDropdownDrawer : PropertyDrawer
{
    private static List<Type> _types;
    private static string[] _typeNames;

    static RequirementDropdownDrawer()
    {
        _types = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                !t.IsAbstract &&
                typeof(AbilityRequirement).IsAssignableFrom(t))
            .ToList();

        _typeNames = _types
            .Select(t => t.Name)
            .ToArray();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Foldout
        property.isExpanded = EditorGUI.Foldout(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            property.isExpanded,
            label,
            true);

        if (!property.isExpanded)
        {
            EditorGUI.EndProperty();
            return;
        }

        EditorGUI.indentLevel++;

        float y = position.y + EditorGUIUtility.singleLineHeight + 2;

        // 🔹 Current type
        Type currentType = GetManagedReferenceType(property);

        int currentIndex = currentType != null
            ? _types.IndexOf(currentType)
            : -1;

        // 🔹 Dropdown
        int selectedIndex = EditorGUI.Popup(
            new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight),
            $"Type ({currentType?.Name ?? "None"})",
            currentIndex,
            _typeNames);

        y += EditorGUIUtility.singleLineHeight + 2;

        // 🔹 If changed → create new instance
        if (selectedIndex >= 0 && selectedIndex != currentIndex)
        {
            var instance = Activator.CreateInstance(_types[selectedIndex]);
            property.managedReferenceValue = instance;
        }

        // 🔹 Draw fields
        if (property.managedReferenceValue != null)
        {
            var child = property.Copy();
            var end = child.GetEndProperty();

            bool enterChildren = true;

            while (child.NextVisible(enterChildren) && !SerializedProperty.EqualContents(child, end))
            {
                enterChildren = false;

                float height = EditorGUI.GetPropertyHeight(child, true);

                EditorGUI.PropertyField(
                    new Rect(position.x, y, position.width, height),
                    child,
                    true);

                y += height + 2;
            }
        }

        EditorGUI.indentLevel--;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        float height = EditorGUIUtility.singleLineHeight; // foldout
        height += EditorGUIUtility.singleLineHeight + 2;  // dropdown

        if (property.managedReferenceValue != null)
        {
            var child = property.Copy();
            var end = child.GetEndProperty();

            bool enterChildren = true;

            while (child.NextVisible(enterChildren) && !SerializedProperty.EqualContents(child, end))
            {
                enterChildren = false;

                height += EditorGUI.GetPropertyHeight(child, true) + 2;
            }
        }

        return height;
    }

    private Type GetManagedReferenceType(SerializedProperty property)
    {
        if (string.IsNullOrEmpty(property.managedReferenceFullTypename))
            return null;

        var parts = property.managedReferenceFullTypename.Split(' ');
        if (parts.Length != 2)
            return null;

        var assemblyName = parts[0];
        var typeName = parts[1];

        return AppDomain.CurrentDomain
            .GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == assemblyName)
            ?.GetType(typeName);
    }
}