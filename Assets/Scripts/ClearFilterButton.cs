using UnityEngine;
using UnityEngine.UI;

// Put on a Button. Hidden until the filter's timer runs out; clicking it removes
// the filter, hides the prompt text, and hides itself.
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(CanvasGroup))]
public class ClearFilterButton : MonoBehaviour
{
    public FilterApplier filter;
    public PromptDisplay prompt;

    CanvasGroup group;

    void Awake()
    {
        // Hidden with a CanvasGroup instead of SetActive so Update keeps running.
        group = GetComponent<CanvasGroup>();
        Show(false);
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void Update()
    {
        if (group.alpha == 0f && filter.timer.IsDone && prompt.enabled) Show(true);
    }

    void OnClick()
    {
        filter.RemoveFilter();
        prompt.enabled = false;
        prompt.label.enabled = false;
        gameObject.SetActive(false);
    }

    void Show(bool on)
    {
        group.alpha = on ? 1f : 0f;
        group.interactable = on;
        group.blocksRaycasts = on;
    }
}
