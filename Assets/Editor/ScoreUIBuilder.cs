using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

// One-time helper: open the Score scene, then Tools > Build Score UI.
public static class ScoreUIBuilder
{
    [MenuItem("Tools/Build Score UI")]
    static void Build()
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null) { Debug.LogError("No Canvas in scene"); return; }

        TMP_DefaultControls.Resources res = new TMP_DefaultControls.Resources();
        res.standard = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");

        for (int i = 0; i < 3; i++)
        {
            GameObject row = new GameObject("Player" + (i + 1) + "Score", typeof(RectTransform));
            row.transform.SetParent(canvas.transform, false);
            ((RectTransform)row.transform).anchoredPosition = new Vector2(0, 100 - i * 100);
            PlayerScoreUI ui = row.AddComponent<PlayerScoreUI>();
            ui.playerIndex = i;

            GameObject text = TMP_DefaultControls.CreateText(res);
            text.name = "Label";
            text.transform.SetParent(row.transform, false);
            RectTransform textRect = (RectTransform)text.transform;
            textRect.sizeDelta = new Vector2(250, 50);
            textRect.anchoredPosition = new Vector2(-100, 0);
            ui.label = text.GetComponent<TMP_Text>();
            ui.label.text = "Player " + (i + 1) + ": 0";

            MakeButton(res, row.transform, ui, "-", -1, 80);
            MakeButton(res, row.transform, ui, "+", 1, 140);

            Undo.RegisterCreatedObjectUndo(row, "Build Score UI");
        }

        EditorSceneManager.MarkSceneDirty(canvas.gameObject.scene);
    }

    static void MakeButton(TMP_DefaultControls.Resources res, Transform parent, PlayerScoreUI ui, string label, int amount, float x)
    {
        GameObject go = TMP_DefaultControls.CreateButton(res);
        go.name = label == "+" ? "Plus" : "Minus";
        go.transform.SetParent(parent, false);
        RectTransform rect = (RectTransform)go.transform;
        rect.sizeDelta = new Vector2(50, 50);
        rect.anchoredPosition = new Vector2(x, 0);
        go.GetComponentInChildren<TMP_Text>().text = label;
        UnityEventTools.AddIntPersistentListener(go.GetComponent<Button>().onClick, ui.Add, amount);
    }
}
