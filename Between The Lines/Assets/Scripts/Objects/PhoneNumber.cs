using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhoneNumber : MonoBehaviour
{
    [SerializeField] Sprite characterBackground;
    [SerializeField] private TextMeshPro nameText;

    [SerializeField] DialogueStage[] dialogueEntries;

    private int dialogueIndex = 0;

    public void Call()
    {
        // TODO: initialize phone scene
        CameraManager.Instance.GoToPhone();
        DialogueManager.Instance.SetBackground(characterBackground);
        DialogueManager.Instance.TriggerDialogue(dialogueEntries[dialogueIndex]);
    }

    public void SetDialogueIndex(int dialogueIndex)
    {
        this.dialogueIndex = dialogueIndex;
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public string GetVisibleName()
    {
        return nameText.text;
    }
}