using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public static DialogController Instance { get; private set; } //Singleton instance

    public GameObject dialogPanel;
    public TMP_Text dialogText, nameText;
    public Image portraitImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    public void showDialogUI(bool show)
    {
        dialogPanel.SetActive(show);
    }


    public void SetNPCInfo(string npcName, Sprite portrait)
    {
        nameText.text = npcName;
        portraitImage.sprite = portrait;
    }


    public void SetDialogText(string text)
    {
        dialogText.text = text;
    }
}
