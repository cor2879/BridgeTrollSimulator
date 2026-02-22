#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
public static class ForceAssetWarmup
{
    static ForceAssetWarmup()
    {
        EditorApplication.delayCall += () =>
        {
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        };
    }
}
#endif