using TMPro;
using UnityEngine;

public class RoleAssignUI : MonoBehaviour
{
    public TMP_Text label;

    void Start() { Refresh(); }

    public void AssignJudge()
    {
        RoleData.Judge = Random.Range(0, 3);
        RoleData.OriginalDrawer = -1;
        RoleData.Cloner = -1;
        Refresh();
    }

    public void AssignRest()
    {
        if (RoleData.Judge < 0) return;
        int a = (RoleData.Judge + 1) % 3;
        int b = (RoleData.Judge + 2) % 3;
        if (Random.value < 0.5f) { RoleData.OriginalDrawer = a; RoleData.Cloner = b; }
        else { RoleData.OriginalDrawer = b; RoleData.Cloner = a; }
        Refresh();
    }

    void Refresh()
    {
        label.text = "Judge: " + Name(RoleData.Judge)
            + "\nOriginal Drawer: " + Name(RoleData.OriginalDrawer)
            + "\nCloner: " + Name(RoleData.Cloner);
    }

    static string Name(int player) { return player < 0 ? "?" : "Player " + (player + 1); }
}
