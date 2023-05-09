using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhoneNumber : MonoBehaviour
{
    [SerializeField] Sprite characterBackground;
    [SerializeField] private TextMeshPro nameText;

    [SerializeField] DialogueStage[] dialogueEntries;

    [SerializeField] private bool moveOnWhenDone;

    private int dialogueIndex = 0;

    public void Call()
    {
        // TODO: initialize phone scene
        CameraManager.Instance.GoToPhone();
        DialogueManager.Instance.SetBackground(characterBackground);
        DialogueManager.Instance.TriggerDialogue(dialogueEntries[dialogueIndex]);
        WatchManager.Instance.NextTurn();

        if (moveOnWhenDone)
        {
            if (dialogueIndex < dialogueEntries.Length - 1)
            {
                dialogueIndex++;
            }
        }
        Notebook.Instance.DisablePhonebook();
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