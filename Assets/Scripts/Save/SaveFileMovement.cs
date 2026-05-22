using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SaveFileMovement : MonoBehaviour
{
    [SerializeField] GameObject[] saveSlots;
    [SerializeField] TextMeshProUGUI[] actionTexts;

    private int currFileIndex = 0;
    private int currActionIndex = 0;

    [SerializeField] private Sprite selectedSrc;
    [SerializeField] private Sprite unselectedSrc;

    Color hoveredColor;
    Color hoveredIndexColor;

    enum MenuState
    {
        FileSelect,
        ActionSelect
    }
    private MenuState currentState = MenuState.FileSelect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ColorUtility.TryParseHtmlString("#ECE72B", out hoveredColor);
        ColorUtility.TryParseHtmlString("#401F0A", out hoveredIndexColor);

        HoverFile(0);
    }


    void MoveFile(int dir)
    {
        currFileIndex += dir;

        if (currFileIndex < 0) {
            currFileIndex = saveSlots.Length - 1;
        }

        if (currFileIndex > saveSlots.Length - 1) {
            currFileIndex = 0;
        }
        SoundEffectManager.Play("InvMovement");
        HoverFile(currFileIndex);
    }


    void MoveAction(int dir)
    {
        currActionIndex += dir;

        if (currActionIndex < 0)
        {
            currActionIndex = actionTexts.Length - 1;
        }

        if (currActionIndex > actionTexts.Length - 1)
        {
            currActionIndex = 0;
        }
        SoundEffectManager.Play("InvMovement");
        HoverAction(currActionIndex);
    }


    void HoverFile(int index)
    {
        for (int i = 0; i < saveSlots.Length; i++) {
            bool hovered = i == index;

            //Get bg
            Image dataPanelImage = saveSlots[i]
                .transform
                .Find("DataPanel")
                .GetComponent<Image>();

            //Get arrow
            Image arrowImage = saveSlots[i]
                .transform
                .Find("Arrow")
                .GetComponent<Image>();

            //Get texts
            TextMeshProUGUI slotIndex = saveSlots[i]
                .transform
                .Find("DataPanel/Index")
                .GetComponent<TextMeshProUGUI>();

            TextMeshProUGUI slotPlyrName = saveSlots[i]
                .transform
                .Find("DataPanel/PlayerName")
                .GetComponent<TextMeshProUGUI>();

            //Change bg
            dataPanelImage.sprite =
                hovered ? selectedSrc : unselectedSrc;

            //Change arrow vis
            Color selectedColor = arrowImage.color;
            selectedColor.a = 1f; // Visible

            Color unselectedColor = arrowImage.color;
            unselectedColor.a = 0f; // Transparente

            arrowImage.color = hovered ? selectedColor : unselectedColor;


            //Change Text color
            slotIndex.color = hovered ? hoveredIndexColor : Color.black;
            slotPlyrName.color = hovered ? hoveredColor : Color.black;
        }
    }


    void HoverAction(int index)
    {
        for (int i = 0; i < actionTexts.Length; i++)
        {
            bool hovered = i == index;

            //Change Text color
            actionTexts[i].color = hovered ? hoveredColor : Color.black;
        }
    }

    void CleanActionTexts()
    {
        for (int i = 0; i < actionTexts.Length; i++)
        {
            //Change Text color
            actionTexts[i].color = Color.black;
        }
        currActionIndex = 0;
    }

    public void OnNavigate(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();

        if (currentState == MenuState.FileSelect)
        {
            if (input.y > 0)
            {
                MoveFile(-1);
            }

            if (input.y < 0)
            {
                MoveFile(1);
            }
        }


        if (currentState == MenuState.ActionSelect)
        {
            if (input.x > 0)
            {
                MoveAction(1);
            }

            if (input.x < 0)
            {
                MoveAction(-1);
            }
        }

    }


    public void OnSubmit(InputAction.CallbackContext ctx)
    {

        if (!ctx.performed) return;

        if (currentState == MenuState.FileSelect)
        {
            currentState = MenuState.ActionSelect;
            SoundEffectManager.Play("ItemSelect");
            HoverAction(0);
        }
        else if (currentState == MenuState.ActionSelect)
        {
            //PerformAction
            SoundEffectManager.Play("ItemSelect");
            PerformAction();
        }

    }


    public void OnCancel(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (currentState == MenuState.FileSelect)
        {
            //Return to title screen
            Debug.Log("Returning to title screen");
        }
        else if (currentState == MenuState.ActionSelect)
        {
            currentState = MenuState.FileSelect;
            CleanActionTexts();
        }
        SoundEffectManager.Play("Cancel");
    }

    public void PerformAction()
    {
        Debug.Log("Performed: " + actionTexts[currActionIndex].text);
    }
}
