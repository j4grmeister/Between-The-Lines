using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private TextMeshPro prompt;
    [SerializeField] private GameObject responsePrefab;
    [SerializeField] private Vector2 topResponsePosition;
    [SerializeField] private Vector2 responseOffset;

    private List<ClickableArea> responseAreas = new List<ClickableArea>();

    public void TriggerDialogue(DialogueStage dialogueStage)
    {
        ResetResponses();
        prompt.text = dialogueStage.prompt;

        for(int i = 0; i < dialogueStage.responses.Length; i++)
        {
            if (dialogueStage.responses[i].requiredClue != "" && !Notebook.Instance.IsClueDiscovered(dialogueStage.responses[i].requiredClue))
                continue;
            int j = i;

            GameObject responseObject = GameObject.Instantiate(responsePrefab, transform) as GameObject;
            responseObject.transform.localPosition = topResponsePosition + responseOffset * j;
            ClickableArea responseArea = responseObject.GetComponentInChildren<ClickableArea>();
            TextMeshPro textMesh = responseObject.GetComponentInChildren<TextMeshPro>();
            textMesh.text = dialogueStage.responses[j].response;

            
            if (dialogueStage.responses[j].nextStage == null)
            {
                responseArea.onClick.AddListener(() => {CameraManager.Instance.GoToPaper();});
            }
            else
            {
                responseArea.onClick.AddListener(() => {TriggerDialogue(dialogueStage.responses[j].nextStage);});
            }
            
            responseArea.onClick.AddListener(() => {dialogueStage.responses[j].onContinue.Invoke();});
            //responseArea.onClick.AddListener(ResetResponses);

            responseAreas.Add(responseArea);
        }
    }

    void ResetResponses()
    {
        while(responseAreas.Count > 0)
        {
            GameObject.Destroy(responseAreas[0].transform.parent.gameObject);
            responseAreas.RemoveAt(0);
        }
    }
}