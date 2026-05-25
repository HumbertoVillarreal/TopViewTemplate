using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SaveFileMenuController : MonoBehaviour
{
    [SerializeField] private SaveFileSlot[] saveSlots;
    [SerializeField] private ActionButtonUI[] actionButtons;
    [SerializeField] private ActionButtonUI[] confirmationButtons;

    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject TitleScreen;
    [SerializeField] private GameObject ConfirmationScreen;
    [SerializeField] private GameObject PlayerNameScreen;
    [SerializeField] private KeyboardController keyboardController;

    private int currFileIndex = 0;
    private int currActionIndex = 0;
    private int currChoiceIndex = 0;

    private int fromSlot = 0;
    private int toSlot = 0;

    private MenuState currentState = MenuState.TitleScreen;
    private HoldState holdState = HoldState.Clean;

    [SerializeField] private PlayerInput playerInput;

    public MenuState CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    public enum MenuState
    {
        FileSelect,
        ActionSelect,
        TitleScreen,
        CopyState,
        DeleteState,
        PlayerNameState,
        ConfirmState
    }


    public enum HoldState
    {
        Clean,
        Delete,
        Copy
    }

    private void Start()
    {
        keyboardController.enabled = false;

        playerInput.SwitchCurrentActionMap("UI");

        HoverFile(currFileIndex);
        CleanActions();
    }

    // ---------------- FILES ----------------

    void MoveFile(int dir)
    {
        currFileIndex += dir;

        if (currFileIndex < 0)
            currFileIndex = saveSlots.Length - 1;

        if (currFileIndex >= saveSlots.Length)
            currFileIndex = 0;

        HoverFile(currFileIndex);
    }

    void HoverFile(int index)
    {
        for (int i = 0; i < saveSlots.Length; i++)
        {
            saveSlots[i].SetHovered(i == index);
        }
    }

    // ---------------- ACTIONS ----------------

    void MoveAction(int dir)
    {
        do
        {
            currActionIndex += dir;

            if (currActionIndex < 0)
                currActionIndex = actionButtons.Length - 1;

            if (currActionIndex >= actionButtons.Length)
                currActionIndex = 0;

        } while (!actionButtons[currActionIndex].IsEnabled);

        HoverAction(currActionIndex);
    }

    void HoverAction(int index)
    {
        for (int i = 0; i < actionButtons.Length; i++)
        {
            actionButtons[i].SetHovered(i == index);
        }
    }

    void CleanActions()
    {
        for (int i = 0; i < actionButtons.Length; i++)
        {
            actionButtons[i].SetHovered(false);
        }
        currActionIndex = 0;
    }

    void PerformAction()
    {
        switch (currActionIndex)
        {
            case 0: //START GAME
                if (SaveController.SaveExists(currFileIndex))
                {
                    SaveController.SelectedSlot = currFileIndex;
                    SceneManager.LoadScene("Game");
                }
                else
                {
                    keyboardController.enabled = true;
                    SaveController.SelectedSlot = currFileIndex;
                    currentState = MenuState.PlayerNameState;
                    PlayerNameScreen.SetActive(true);
                    //SceneManager.LoadScene("Game");
                }
                    break;

            case 1: //COPY
                StartCopyProcess();

                Debug.Log("COPY SAVE");
                break;

            case 2: //DELETE
                StartDeleteProcess();
                //DeleteFile();
                break;
        }
    }

    private void DeleteFile()
    {
        SaveController.DeleteSaveFile(currFileIndex);
        saveSlots[currFileIndex].VerifyFile();

        CleanActions();
        currentState = MenuState.FileSelect;

        ConfirmationScreen.SetActive(false);

        Debug.Log("DELETE SAVE");
    }

    // ---------------- INPUT ----------------

    public void OnNavigate(InputAction.CallbackContext ctx)
    {
        Debug.Log("Navigate");

        Vector2 input = ctx.ReadValue<Vector2>();
        bool moved = false;

        if (currentState == MenuState.FileSelect)
        {
            if (input.y > 0)
            {
                MoveFile(-1);
                moved = true;
            }

            if (input.y < 0)
            {
                MoveFile(1);
                moved = true;
            }
        }
        else if (currentState == MenuState.ActionSelect)
        {
            if (input.x > 0)
            {
                MoveAction(1);
                moved = true;
            }

            if (input.x < 0) {
            MoveAction(-1);
            moved = true;
        }

        }
        else if (currentState == MenuState.CopyState)
        {
            if (input.y > 0)
            {
                MoveFile(-1);
                moved = true;
            }

            if (input.y < 0)
            {
                MoveFile(1);
                moved = true;
            }
        }
        else if (currentState == MenuState.ConfirmState)
        {
            if (input.x > 0)
            {
                MoveConfirmChoice(-1);
                moved = true;
            }

            if (input.x < 0)
            {
                MoveConfirmChoice(1);
                moved = true;
            }
        }
        else if (currentState == MenuState.PlayerNameState)
        {
            keyboardController.Navigate(ctx);
        }
        if (moved)
            SoundEffectManager.Play("InvMovement");
    }

    public void OnSubmit(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        Debug.Log("Submit");

        if (currentState == MenuState.FileSelect)
        {
            var fileExists = saveSlots[currFileIndex].VerifyFile();
            //Debug.Log("File exists: " + fileExists);

            actionButtons[1].SetEnabled(fileExists);
            actionButtons[2].SetEnabled(fileExists);

            currentState = MenuState.ActionSelect;
            SaveController.SelectedSlot = currFileIndex;

            SoundEffectManager.Play("ItemSelect");

            HoverAction(currActionIndex);
        }
        else if (currentState == MenuState.TitleScreen)
        {
            currentState = MenuState.FileSelect;
            TitleScreen.SetActive(false);
            MainMenu.SetActive(true);
        }
        else if (currentState == MenuState.ActionSelect) {
            SoundEffectManager.Play("ItemSelect");
            PerformAction();
        }
        else if (currentState == MenuState.PlayerNameState)
        {
            SoundEffectManager.Play("ItemSelect");
            keyboardController.Submit();
        }
        else if (currentState == MenuState.CopyState)
        {
            ConfirmationScreen.SetActive(true);

            currentState = MenuState.ConfirmState;
            holdState = HoldState.Copy;
        }

        else if (currentState == MenuState.ConfirmState)
        {
            CheckConfirm();
        }
        else if (currentState == MenuState.DeleteState)
        {
            ConfirmationScreen.SetActive(true);

            currentState = MenuState.ConfirmState;
            holdState = HoldState.Delete;

        }
    }

    public void OnCancel(InputAction.CallbackContext ctx)
    {
        Debug.Log("Cancel");

        if (!ctx.performed) return;

        if (currentState == MenuState.ActionSelect)
        {
            currFileIndex = 0;
            currentState = MenuState.FileSelect;

            CleanActions();
        }
        else if (currentState == MenuState.DeleteState)
        {

        }
        else if (currentState == MenuState.CopyState)
        {
            currFileIndex = fromSlot;
            HoverFile(currFileIndex);
            fromSlot = 0;

            actionButtons[1].SetEnabled(true);
            actionButtons[2].SetEnabled(true);

            CleanActions();

            currentState = MenuState.ActionSelect;
        }
        else if (currentState == MenuState.PlayerNameState)
        {
            PlayerNameScreen.SetActive(false);
            currentState = MenuState.ActionSelect;
            keyboardController.Cancel();
            keyboardController.enabled = false;
        }
        else if (currentState == MenuState.ConfirmState)
        {
            ConfirmationScreen.SetActive(false);
            currentState = MenuState.ActionSelect;
            holdState = HoldState.Clean;
        }
        else
        {
            currentState = MenuState.TitleScreen;
            MainMenu.SetActive(false);
            TitleScreen.SetActive(true);
        }

        SoundEffectManager.Play("Cancel");
    }


    public void StartCopyProcess()
    {
        currentState = MenuState.CopyState;

        fromSlot = currFileIndex;

    }


    public void StartDeleteProcess()
    {
        ConfirmationScreen.SetActive(true);

        currentState = MenuState.ConfirmState;
        holdState = HoldState.Delete;
        
    }


    public void CopyFile()
    {

        toSlot = currFileIndex;

        SaveController.CopySaveFile(fromSlot, toSlot);

        currentState = MenuState.FileSelect;

        fromSlot = 0;
        toSlot = 0;

        CleanActions();

        for (int i = 0; i < saveSlots.Length; i++)
        {
            saveSlots[i].VerifyFile();
        }
    }


    void MoveConfirmChoice(int dir)
    {
        currChoiceIndex += dir;

        if (currChoiceIndex < 0)
            currChoiceIndex = confirmationButtons.Length - 1;

        if (currChoiceIndex >= confirmationButtons.Length)
            currChoiceIndex = 0;

        HoverChoice(currChoiceIndex);
    }


    void HoverChoice(int index)
    {
        for (int i = 0; i < confirmationButtons.Length; i++)
        {
            confirmationButtons[i].SetHovered(i == index);
        }
    }

    void CheckConfirm()
    {
        if (currChoiceIndex == 0)
        {

        }
        else
        {
            if (holdState == HoldState.Copy)
            {
                CopyFile();
            }
            else if (holdState == HoldState.Delete)
            {
                DeleteFile();
            }
        }

        ConfirmationScreen.SetActive(false);
        currChoiceIndex = 0;
        currentState = MenuState.FileSelect;
        CleanActions();
    }

}
