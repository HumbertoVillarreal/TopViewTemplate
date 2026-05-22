using TMPro;
using UnityEngine;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private Color hoveredColor;
    private Color disabledColor;

    private bool isEnabled = true;
    private bool isHovered = false;
    public bool IsEnabled => isEnabled;


    private void Awake()
    {
        ColorUtility.TryParseHtmlString("#6D6F09", out hoveredColor);
        ColorUtility.TryParseHtmlString("#676767", out disabledColor);
    }


    public void SetHovered(bool hovered)
    {
        isHovered = hovered;
        UpdateVisual();
    }

    public void SetEnabled(bool enabled)
    {
        isEnabled = enabled;
        UpdateVisual();
    }


    private void UpdateVisual()
    {
        if (!isEnabled)
        {
            Color color = disabledColor;
            color.a = 134f / 255f;

            text.color = color;
        }
        else
        {
            text.color = isHovered ? hoveredColor : Color.black;
        }
    }
}
