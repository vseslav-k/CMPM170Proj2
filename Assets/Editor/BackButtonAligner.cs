using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

// Tools > Align Back Buttons: in every build scene, any Button that calls LoadMainMenu
// gets anchored to the top-left corner with the same offset.
public static class BackButtonAligner
{
    static readonly Vector2 offset = new Vector2(20, -20);

    [MenuItem("Tools/Align Back Buttons")]
    static void Align()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        string startScene = EditorSceneManager.GetActiveScene().path;

        foreach (EditorBuildSettingsScene s in EditorBuildSettings.scenes)
        {
            var scene = EditorSceneManager.OpenScene(s.path);
            int count = 0;
            foreach (Button b in Object.FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (!CallsMainMenu(b)) continue;
                RectTransform rt = (RectTransform)b.transform;
                rt.anchorMin = new Vector2(0, 1);
                rt.anchorMax = new Vector2(0, 1);
                rt.pivot = new Vector2(0, 1);
                rt.anchoredPosition = offset;
                b.gameObject.name = "Back";
                count++;
            }
            if (count > 0)
            {
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
            }
            Debug.Log(s.path + ": aligned " + count + " back button(s)");
        }

        if (!string.IsNullOrEmpty(startScene)) EditorSceneManager.OpenScene(startScene);
    }

    static bool CallsMainMenu(Button b)
    {
        for (int i = 0; i < b.onClick.GetPersistentEventCount(); i++)
            if (b.onClick.GetPersistentMethodName(i) == "LoadMainMenu") return true;
        return false;
    }
}
