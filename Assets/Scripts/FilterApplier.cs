using UnityEngine;

// Loads every texture in Resources/Filters. When the timer ends, puts a random one on the sheet.
public class FilterApplier : MonoBehaviour
{
    public DrawTimer timer;
    public PaintSheet sheet;

    Texture2D[] filters;
    bool applied;

    void Awake()
    {
        filters = Resources.LoadAll<Texture2D>("Filters");
    }

    void Update()
    {
        if (applied || !timer.IsDone) return;
        applied = true;

        if (filters.Length == 0)
        {
            Debug.LogWarning("No filters in Resources/Filters");
            return;
        }
        sheet.SetMask(filters[Random.Range(0, filters.Length)]);
    }

    public void RemoveFilter()
    {
        sheet.SetMask(false);
    }
}
