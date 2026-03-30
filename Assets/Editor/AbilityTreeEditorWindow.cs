using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Requirements;
/*
public class AbilityTreeEditorWindow : EditorWindow
{
    private AbilityTree selectedTree;
    private SerializedObject serializedTree;

    [MenuItem("Tools/Ability Tree Editor")]
    public static void Open()
    {
        GetWindow<AbilityTreeEditorWindow>("Ability Tree Editor");
    }

    private void OnGUI()
    {
        DrawToolbar();

        if (selectedTree == null)
        {
            EditorGUILayout.HelpBox("Select an AbilityTree asset.", MessageType.Info);
            return;
        }

        if (selectedTree.Tiers.Count == 0)
        {
            if (GUILayout.Button("Create First Tier"))
            {
                Undo.RecordObject(selectedTree, "Add Tier");

                selectedTree.AddTier(); // you'll implement this helper

                EditorUtility.SetDirty(selectedTree);
            }
        }

        if (selectedTree != null)
        {
            if (serializedTree == null || serializedTree.targetObject != selectedTree)
            {
                serializedTree = new SerializedObject(selectedTree);
            }

            serializedTree.Update();
        }

        DrawNodes();

        if (serializedTree != null)
        {
            serializedTree.ApplyModifiedProperties();
        }
    }

    private void DrawToolbar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        var newTree = (AbilityTree)EditorGUILayout.ObjectField(
            selectedTree,
            typeof(AbilityTree),
            false,
            GUILayout.Width(300));

        if (newTree != selectedTree)
        {
            selectedTree = newTree;

            if (selectedTree != null)
                serializedTree = new SerializedObject(selectedTree);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void AddTier()
    {
        Undo.RecordObject(selectedTree, "Add Tier");

        var tiersField = typeof(AbilityTree)
            .GetField("tiers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var tiers = (List<AbilityTier>)tiersField.GetValue(selectedTree);

        var newTier = new AbilityTier
        {
            tierId = tiers.Count,
            name = $"Tier {tiers.Count}",
            nodes = new List<AbilityNode>()
        };

        tiers.Add(newTier);

        EditorUtility.SetDirty(selectedTree);
    }

    private void AddNode()
    {
        if (selectedTree.Tiers == null || selectedTree.Tiers.Count == 0)
        {
            Debug.LogWarning("Add a tier first.");
            return;
        }

        Undo.RecordObject(selectedTree, "Add Node");

        var tier = selectedTree.Tiers[0]; // default to Tier 0 (we'll improve this later)

        var node = new AbilityNode
        {
            id = System.Guid.NewGuid().ToString(),
            tier = tier.tierId,
            treeId = selectedTree.Id,
            position = new Vector2(200, 200),
            requirements = new List<AbilityRequirement>()
        };

        tier.nodes.Add(node);

        EditorUtility.SetDirty(selectedTree);
    }

    private void DrawNodes()
    {
        BeginWindows();

        float yOffset = 50f;
        float tierHeight = 200f;

        for (int i = 0; i < selectedTree.Tiers.Count; i++)
        {
            var tier = selectedTree.Tiers[i];

            DrawTierHeader(tier, i, yOffset);

            if (tier?.nodes != null)
            {
                foreach (var node in tier.nodes)
                {
                    if (node == null) continue;

                    var rect = new Rect(node.position, new Vector2(200, 120));

                    rect = GUI.Window(
                        node.id != null ? node.id.GetHashCode() : node.GetHashCode(),
                        rect,
                        (id) => DrawNodeWindow(id, node),
                        node.ability != null ? node.ability.Name : "NULL");

                    // node.position = rect.position;
                }
            }

            yOffset += tierHeight;
        }

        EndWindows();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(selectedTree);
        }
    }

    private void DrawTierHeader(AbilityTier tier, int index, float y)
    {
        if (serializedTree == null)
            return;

        Rect rect = new Rect(10, y - 30, 400, 100);

        GUILayout.BeginArea(rect, EditorStyles.helpBox);

        // 🔹 Top Row
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label($"Tier {index}", GUILayout.Width(60));

        tier.name = EditorGUILayout.TextField(tier.name);

        if (GUILayout.Button("+ Node", GUILayout.Width(80)))
        {
            AddNodeToTier(index);
        }

        EditorGUILayout.EndHorizontal();

        // 🔹 Requirements (OUTSIDE horizontal 🔥)
        var tiersProp = serializedTree.FindProperty("tiers");
        var tierProp = tiersProp.GetArrayElementAtIndex(index);
        var requirementsProp = tierProp.FindPropertyRelative("requirements");

        EditorGUILayout.Space(4);
        EditorGUILayout.LabelField("Requirements", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(requirementsProp, true);

        GUILayout.EndArea();
    }

    private void DrawNodeWindow(int id, AbilityNode node)
    {
        try
        {
            if (serializedTree == null || node == null)
                return;

            GUILayout.BeginVertical("box");

            EditorGUI.BeginChangeCheck();

            // 🔹 ID
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Id", GUILayout.Width(30));
            node.id = EditorGUILayout.TextField(node.id);
            EditorGUILayout.EndHorizontal();

            // 🔹 Ability
            node.ability = (Ability)EditorGUILayout.ObjectField(
                "Ability",
                node.ability,
                typeof(Ability),
                false);

            EditorGUILayout.LabelField($"Tier: {node.tier}");

            // 🔥 SAFE serialized access
            var tiersProp = serializedTree.FindProperty("tiers");

            if (tiersProp != null)
            {
                var nodeProp = FindNodeProperty(tiersProp, node);

                if (nodeProp != null)
                {
                    var reqProp = nodeProp.FindPropertyRelative("requirements");

                    if (reqProp != null)
                    {
                        EditorGUILayout.Space(5);
                        EditorGUILayout.LabelField("Requirements", EditorStyles.boldLabel);
                        EditorGUILayout.PropertyField(reqProp, true);
                    }
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(selectedTree);
            }

            GUI.DragWindow();
        }
        finally
        {
            GUILayout.EndVertical(); // 🔥 ALWAYS closes
        }
    }

    private void AddNodeToTier(int tierIndex)
    {
        Undo.RecordObject(selectedTree, "Add Node");

        var tier = selectedTree.Tiers[tierIndex];

        var node = new AbilityNode
        {
            id = System.Guid.NewGuid().ToString(),
            tier = tierIndex,
            treeId = selectedTree.Id,
            // position = new Vector2(300, 100 + (tierIndex * 200)),
            requirements = new List<AbilityRequirement>()
        };

        tier.nodes.Add(node);

        EditorUtility.SetDirty(selectedTree);
    }

    private SerializedProperty FindNodeProperty(SerializedProperty tiersProp, AbilityNode targetNode)
    {
        if (tiersProp == null)
            return null;

        for (int i = 0; i < tiersProp.arraySize; i++)
        {
            var tier = tiersProp.GetArrayElementAtIndex(i);
            var nodes = tier.FindPropertyRelative("nodes");

            for (int j = 0; j < nodes.arraySize; j++)
            {
                var node = nodes.GetArrayElementAtIndex(j);

                var idProp = node.FindPropertyRelative("id");

                if (idProp.stringValue == targetNode.id)
                    return node;
            }
        }

        return null;
    }

    private void HandleNodeContextMenu(AbilityNode node)
    {
        Event e = Event.current;

        if (e.type == EventType.ContextClick)
        {
            Rect windowRect = new Rect(node.position, new Vector2(150, 60));

            if (windowRect.Contains(e.mousePosition))
            {
                GenericMenu menu = new GenericMenu();

                menu.AddItem(new GUIContent("Delete Node"), false, () =>
                {
                    DeleteNode(node);
                });

                menu.ShowAsContext();
                e.Use();
            }
        }
    }

    private void DeleteNode(AbilityNode node)
    {
        Undo.RecordObject(selectedTree, "Delete Node");

        foreach (var tier in selectedTree.Tiers)
        {
            if (tier.nodes.Remove(node))
                break;
        }

        EditorUtility.SetDirty(selectedTree);
    }

    private void DrawConnections()
    {
        if (selectedTree == null) return;

        var lookup = BuildNodeLookup();

        Handles.BeginGUI();

        foreach (var tier in selectedTree.Tiers)
        {
            if (tier?.nodes == null) continue;

            foreach (var node in tier.nodes)
            {
                if (node == null) continue;

                if (node.requirements == null) continue;

                foreach (var req in node.requirements)
                {
                    // 🔥 Only care about HasAbilityRequirement
                    if (req is HasAbilityRequirement hasReq)
                    {
                        var sourceNode = hasReq.RequiredNode; // we'll expose this in a sec

                        if (sourceNode == null) continue;

                        if (!lookup.TryGetValue(sourceNode.id, out var fromNode))
                            continue;

                        DrawConnection(fromNode, node);
                    }
                }
            }
        }

        Handles.EndGUI();
    }

    private void DrawConnection(AbilityNode from, AbilityNode to)
    {
        // Vector3 start = from.position + new Vector2(75, 30); // center of node
        // Vector3 end = to.position + new Vector2(75, 30);

        Vector3 startTangent = Vector3.zero + Vector3.right * 50f;
        Vector3 endTangent = Vector3.zero + Vector3.left * 50f;

        Handles.DrawBezier(
            Vector3.zero,
            Vector3.zero,
            startTangent,
            endTangent,
            Color.white,
            null,
            3f
        );
    }

    private Dictionary<string, AbilityNode> BuildNodeLookup()
    {
        var dict = new Dictionary<string, AbilityNode>();

        foreach (var tier in selectedTree.Tiers)
        {
            if (tier?.nodes == null) continue;

            foreach (var node in tier.nodes)
            {
                if (node == null || string.IsNullOrEmpty(node.id)) continue;

                dict[node.id] = node;
            }
        }

        return dict;
    }
}
*/