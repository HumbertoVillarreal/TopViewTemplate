using UnityEngine;

[CreateAssetMenu(fileName="NewNPCDialog", menuName="NPC Dialogue")]
public class NPC_Dialog : ScriptableObject
{

    public string npcName;
    public Sprite npcPortraits;
    public string[] dialogLines;
    public bool[] autoProgressLines;
    public bool[] endDialogLlines; //Where dialog ends
    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.5f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;
    public bool singleSound = false;

    public DialogChoice[] choices;
}


[System.Serializable]
public class DialogChoice
{
    public int dialogIndex; //Dialogue line where choices appear
    public string[] choices; //Player response options
    public int[] nextDialogIndexes; //Where choice leads
}