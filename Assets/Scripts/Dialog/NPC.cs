using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, I_Interactable
{

    public NPC_Dialog dialogData;
    private DialogController dialogUI;

    private int dialogIndex;
    private bool isTyping, isDialogActive;


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
        if (dialogData == null || (PauseController.IsGamePaused && !isDialogActive)) { return; }

        if (isDialogActive)
        {
            NextLine(); //Next Line
        }
        else
        {
            StartDialog(); //Start Dialog
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
        StartCoroutine(TypeLine());
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
        else if (++dialogIndex < dialogData.dialogLines.Length)
        {
            StartCoroutine(TypeLine());
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

}
