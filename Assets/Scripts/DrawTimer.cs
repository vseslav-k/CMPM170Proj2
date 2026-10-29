using TMPro;
using UnityEngine;

// Counts down and shows it in label. When it hits 0, every PaintSheet in the scene stops drawing.
public class DrawTimer : MonoBehaviour
{
    public float duration = 45f;
    public TMP_Text label;

    public bool IsDone { get { return done; } }

    float timeLeft;
    bool done;

    void Start()
    {
        timeLeft = duration;
    }

    void Update()
    {
        if (done) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            done = true;
            foreach (PaintSheet sheet in FindObjectsByType<PaintSheet>(FindObjectsSortMode.None))
                sheet.enabled = false;
        }

        label.text = done ? "Time's up!" : Mathf.CeilToInt(timeLeft).ToString();
    }
}
