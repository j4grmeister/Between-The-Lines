using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneNumber : MonoBehaviour
{
    [SerializeField] DialogueStage[] dialogueEntries;

    private int dialogueIndex = 0;

    public void Call()
    {
        // TODO: initialize phone scene
        CameraManager.Instance.GoToPhone();
        DialogueManager.Instance.TriggerDialogue(dialogueEntries[dialogueIndex]);
    }

    public void SetDialogueIndex(int dialogueIndex)
    {
        this.dialogueIndex = dialogueIndex;
    }
}