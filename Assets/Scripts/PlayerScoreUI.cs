using TMPro;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{
    public int playerIndex;
    public TMP_Text label;

    void Start() { Refresh(); }

    public void Add(int amount)
    {
        ScoreData.Points[playerIndex] += amount;
        Refresh();
    }

    void Refresh()
    {
        label.text = "Player " + (playerIndex + 1) + ": " + ScoreData.Points[playerIndex];
    }
}
