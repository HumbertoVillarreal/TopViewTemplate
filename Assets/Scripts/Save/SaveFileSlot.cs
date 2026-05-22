using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileSlot : MonoBehaviour
{
    [SerializeField] private Image dataPanelImage;
    [SerializeField] private Image arrowImage;

    [SerializeField] private int slotNum;

    [SerializeField] private TextMeshProUGUI slotIndex;
    [SerializeField] private TextMeshProUGUI playerName;

    [SerializeField] private Sprite selectedSrc;
    [SerializeField] private Sprite unselectedSrc;

    private Color hoveredColor;
    private Color hoveredIndexColor;


    private void Awake()
    {
        ColorUtility.TryParseHtmlString("#ECE72B", out hoveredColor);
        ColorUtility.TryParseHtmlString("#401F0A", out hoveredIndexColor);

        VerifyFile();
    }

    public void SetHovered(bool hovered)
    {
        dataPanelImage.sprite =
            hovered ? selectedSrc : unselectedSrc;

        Color c = arrowImage.color;
        c.a = hovered ? 1f : 0f;
        arrowImage.color = c;

        slotIndex.color =
            hovered ? hoveredIndexColor : Color.black;

        playerName.color =
            hovered ? hoveredColor : Color.black;
    }


    public bool VerifyFile()
    {
        if (SaveController.SaveExists(slotNum - 1))
        {
            playerName.text =
                SaveController.GetPlayerName(slotNum - 1);

            return true;
        }
        else
        {
            playerName.text = "- - -";

            return false;
        }
    }
}
