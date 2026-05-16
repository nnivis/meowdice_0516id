#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
    [InitializeOnLoad]
    public static class BootstrapPlayMode
    {
        private const string BootstrapScenePath = "Assets/Scenes/BootstrapScene.unity";

        static BootstrapPlayMode()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                var bootstrapScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(BootstrapScenePath);

                if (bootstrapScene == null)
                {
                    Debug.LogError($"Bootstrap scene not found: {BootstrapScenePath}");
                    return;
                }

                EditorSceneManager.playModeStartScene = bootstrapScene;
            }

            if (state == PlayModeStateChange.EnteredEditMode)
            {
                EditorSceneManager.playModeStartScene = null;
            }
        }
    }
}
#endif