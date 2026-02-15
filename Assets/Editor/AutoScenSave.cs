#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;

[InitializeOnLoad]
public static class AutoSceneSave
{
    private static double lastSaveTime;
    private const double saveInterval = 300; // 5 minutes

    static AutoSceneSave()
    {
        EditorApplication.update += Update;
    }

    private static void Update()
    {
        if (EditorApplication.timeSinceStartup - lastSaveTime > saveInterval)
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.SaveOpenScenes();
                Debug.Log("Auto-saved scenes.");
                lastSaveTime = EditorApplication.timeSinceStartup;
            }
        }
    }
}
#endif