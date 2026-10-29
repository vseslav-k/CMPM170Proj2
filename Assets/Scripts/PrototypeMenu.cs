using UnityEngine;

// Barebones prototype menu using IMGUI. Auto-spawns on play, no scene setup needed.
public class PrototypeMenu : MonoBehaviour
{
    enum Screen { Main, AssignRoles, Score, OriginalDrawer, Cloner, Judge }

    Screen current = Screen.Main;
    int[] points = new int[3];
    int judge = -1;
    int originalDrawer = -1;
    int cloner = -1;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Spawn()
    {
        new GameObject("PrototypeMenu").AddComponent<PrototypeMenu>();
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(20, 20, 300, 500));
        switch (current)
        {
            case Screen.Main:
                GUILayout.Label("Main Menu");
                if (GUILayout.Button("Assign Roles")) current = Screen.AssignRoles;
                if (GUILayout.Button("Score")) current = Screen.Score;
                if (GUILayout.Button("Original Drawer Screen")) current = Screen.OriginalDrawer;
                if (GUILayout.Button("Cloner Screen")) current = Screen.Cloner;
                if (GUILayout.Button("Judge Screen")) current = Screen.Judge;
                break;

            case Screen.AssignRoles:
                GUILayout.Label("Assign Roles");
                if (GUILayout.Button("Assign Judge"))
                {
                    judge = Random.Range(0, 3);
                    originalDrawer = -1;
                    cloner = -1;
                }
                if (GUILayout.Button("Assign Rest") && judge >= 0)
                {
                    int a = (judge + 1) % 3;
                    int b = (judge + 2) % 3;
                    if (Random.value < 0.5f) { originalDrawer = a; cloner = b; }
                    else { originalDrawer = b; cloner = a; }
                }
                if (judge >= 0) GUILayout.Label("Judge: Player " + (judge + 1));
                if (originalDrawer >= 0) GUILayout.Label("Original Drawer: Player " + (originalDrawer + 1));
                if (cloner >= 0) GUILayout.Label("Cloner: Player " + (cloner + 1));
                BackButton();
                break;

            case Screen.Score:
                GUILayout.Label("Score");
                for (int i = 0; i < 3; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Player " + (i + 1) + ": " + points[i]);
                    if (GUILayout.Button("-")) points[i]--;
                    if (GUILayout.Button("+")) points[i]++;
                    GUILayout.EndHorizontal();
                }
                BackButton();
                break;

            case Screen.OriginalDrawer:
                GUILayout.Label("Original Drawer Screen");
                BackButton();
                break;

            case Screen.Cloner:
                GUILayout.Label("Cloner Screen");
                BackButton();
                break;

            case Screen.Judge:
                GUILayout.Label("Judge Screen");
                BackButton();
                break;
        }
        GUILayout.EndArea();
    }

    void BackButton()
    {
        if (GUILayout.Button("Back")) current = Screen.Main;
    }
}
