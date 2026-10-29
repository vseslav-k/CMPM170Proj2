using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// One-time helper: open the Roles scene, then Tools > Build Roles UI.
public static class RolesUIBuilder
{
    [MenuItem("Tools/Build Roles UI")]
    static void Build()
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null) { Debug.LogError("No Canvas in scene"); return; }

        TMP_DefaultControls.Resources res = new TMP_DefaultControls.Resources();
        res.standard = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");

        GameObject root = new GameObject("RoleAssign", typeof(RectTransform));
        root.transform.SetParent(canvas.transform, false);
        RoleAssignUI ui = root.AddComponent<RoleAssignUI>();

        GameObject text = TMP_DefaultControls.CreateText(res);
        text.name = "Label";
        text.transform.SetParent(root.transform, false);
        RectTransform textRect = (RectTransform)text.transform;
        textRect.sizeDelta = new Vector2(400, 150);
        textRect.anchoredPosition = new Vector2(0, -60);
        ui.label = text.GetComponent<TMP_Text>();
        ui.label.text = "Judge: ?\nOriginal Drawer: ?\nCloner: ?";

        MakeButton(res, root.transform, "Assign Judge", ui.AssignJudge, -110);
        MakeButton(res, root.transform, "Assign Rest", ui.AssignRest, 110);

        Undo.RegisterCreatedObjectUndo(root, "Build Roles UI");
        EditorSceneManager.MarkSceneDirty(canvas.gameObject.scene);
    }

    static void MakeButton(TMP_DefaultControls.Resources res, Transform parent, string label, UnityAction action, float x)
    {
        GameObject go = TMP_DefaultControls.CreateButton(res);
        go.name = label;
        go.transform.SetParent(parent, false);
        RectTransform rect = (RectTransform)go.transform;
        rect.sizeDelta = new Vector2(200, 50);
        rect.anchoredPosition = new Vector2(x, 60);
        go.GetComponentInChildren<TMP_Text>().text = label;
        UnityEventTools.AddVoidPersistentListener(go.GetComponent<Button>().onClick, action);
    }
}
