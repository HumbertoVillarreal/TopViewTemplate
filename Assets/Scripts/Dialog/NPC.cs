using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class NPC : MonoBehaviour, I_Interactable
{

    public NPC_Dialog dialogData;
    private DialogController dialogUI;

    private int dialogIndex;
    private bool isTyping, isDialogActive;

    private bool isChoosing;
    private bool choiceInputLocked;


    private void Start()
    {
        dialogUI = DialogController.Instance;
    }


    public bool CanInteract()
    {
        return !isDialogActive;
    }

    public void Interact()
    {
        if (dialogData == null || (PauseController.IsGamePaused && !isDialogActive))
            return;

        // Don't let the interaction key advance the dialogue
        // while the player is selecting a choice.
        if (isChoosing)
            return;

        if (isDialogActive)
        {
            NextLine();
        }
        else
        {
            StartDialog();
        }
    }


    void StartDialog()
    {
        isDialogActive = true;
        dialogIndex = 0;

        dialogUI.SetNPCInfo(dialogData.npcName, dialogData.npcPortraits);

        dialogUI.showDialogUI(true);
        PauseController.SetPause(true);

        //Type Line
        DisplayCurrentLine();
    } 


    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogUI.SetDialogText("");

        if (dialogData.singleSound) {

            for (int i = 1; i <= Random.Range(1, 4); i++) {
                SoundEffectManager.PlayVoice(dialogData.voiceSound, dialogData.voicePitch);
                yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
            }
            
        }

        foreach(char letter in dialogData.dialogLines[dialogIndex])
        {
            dialogUI.SetDialogText(dialogUI.dialogText.text += letter);
            if (!dialogData.singleSound)
            {
                SoundEffectManager.PlayVoice(dialogData.voiceSound, dialogData.voicePitch);
            }

            yield return new WaitForSeconds(dialogData.typingSpeed);
        }

        isTyping = false;

        if (dialogData.autoProgressLines.Length > 0 && dialogData.autoProgressLines[dialogIndex]) {
            yield return new WaitForSeconds(dialogData.autoProgressDelay);

            NextLine(); //Display Next Line

        }
    }


    void NextLine()
    {
        if (isTyping) { 
            StopAllCoroutines();
            dialogUI.SetDialogText(dialogData.dialogLines[dialogIndex]);
            isTyping= false;
        }

        //Clear choices
        dialogUI.ClearChoices();

        //Check endDialogLines
        if (dialogData.endDialogLlines.Length > dialogIndex && dialogData.endDialogLlines[dialogIndex])
        {
            EndDialog();
            return;
        }

        //Check choices and show
        foreach(DialogChoice dialogChoice in dialogData.choices)
        {
            if (dialogChoice.dialogIndex == dialogIndex)
            {
                //Display choices
                DisplayChoices(dialogChoice);
                return;
            }
        }


        if (++dialogIndex < dialogData.dialogLines.Length)
        {
            DisplayCurrentLine();
        }
        else
        {
            EndDialog(); //End Dialog
        }
    }


    public void EndDialog()
    {
        StopAllCoroutines();
        isDialogActive = false;
        dialogUI.SetDialogText("");
        dialogUI.showDialogUI(false);
        PauseController.SetPause(false);
    }


    void DisplayChoices(DialogChoice choice)
    {
        isChoosing = true;
        choiceInputLocked = true;

        for (int i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogIndexes[i];

            dialogUI.CreateChoiceButton(
                choice.choices[i],
                () =>
                {
                    if (choiceInputLocked)
                        return;

                    ChooseOption(nextIndex);
                }
            );
        }

        dialogUI.SelectFirstChoice();

        StartCoroutine(WaitForChoiceInputRelease());
    }

    void ChooseOption(int nextIndex)
    {
        isChoosing = false;

        dialogIndex = nextIndex;
        dialogUI.ClearChoices();

        DisplayCurrentLine();
    }

    void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    IEnumerator WaitForChoiceInputRelease()
    {
        // Wait until Space is no longer being held.
        yield return new WaitUntil(() =>
            Keyboard.current != null &&
            !Keyboard.current.spaceKey.isPressed
        );

        choiceInputLocked = false;
    }

}
