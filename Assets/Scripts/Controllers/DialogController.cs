using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public static DialogController Instance { get; private set; } //Singleton instance

    public GameObject dialogPanel;
    public TMP_Text dialogText, nameText;
    public Image portraitImage;
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;

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

    public void ClearChoices()
    {
        foreach(Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public GameObject CreateChoiceButton(string choiceText, UnityEngine.Events.UnityAction onClick)
    {
        GameObject choiceButton = Instantiate(choiceButtonPrefab, choiceContainer);
        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onClick);
        
        return choiceButton;
    }


    public void SelectFirstChoice()
    {
        if (choiceContainer.childCount == 0) return;

        GameObject firstChoice = choiceContainer.GetChild(0).gameObject;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstChoice);
    }
}
