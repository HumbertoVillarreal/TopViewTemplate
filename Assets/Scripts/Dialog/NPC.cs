using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, I_Interactable
{

    public NPC_Dialog dialogData;
    public GameObject dialogPanel;
    public TMP_Text dialogText, nameText;
    public Image portraitImage;


    private int dialogIndex;
    private bool isTyping, isDialogActive;

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

        nameText.SetText(dialogData.npcName);
        portraitImage.sprite = dialogData.npcPortraits;

        dialogPanel.SetActive(true);
        PauseController.SetPause(true);

        //Type Line
        StartCoroutine(TypeLine());
    } 


    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogText.SetText("");

        if (dialogData.singleSound) {

            for (int i = 1; i <= Random.Range(1, 4); i++) {
                SoundEffectManager.PlayVoice(dialogData.voiceSound, dialogData.voicePitch);
                yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
            }
            
        }

        foreach(char letter in dialogData.dialogLines[dialogIndex])
        {
            dialogText.text += letter;
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
            dialogText.SetText(dialogData.dialogLines[dialogIndex]);
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
        dialogText.SetText("");
        dialogPanel.SetActive(false);
        PauseController.SetPause(false);
    }

}
