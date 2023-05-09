using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private TextMeshPro prompt;
    [SerializeField] private GameObject responsePrefab;
    [SerializeField] private Vector2 topResponsePosition;
    [SerializeField] private Vector2 responseOffset;

    [SerializeField] private GameObject paperFromNotebookButton;
    [SerializeField] private GameObject phoneFromNotebookButton;
    [SerializeField] private Transform responsesParent;

    private List<ClickableArea> responseAreas = new List<ClickableArea>();

    public void SetBackground(Sprite backgroundSprite)
    {
        background.sprite = backgroundSprite;
    }

    public void TriggerDialogue(DialogueStage dialogueStage)
    {
        if (!phoneFromNotebookButton.activeSelf)
        {
            paperFromNotebookButton.SetActive(false);
            phoneFromNotebookButton.SetActive(true);
        }

        ResetResponses();
        prompt.text = dialogueStage.prompt;

        for(int i = 0; i < dialogueStage.responses.Length; i++)
        {
            if (dialogueStage.responses[i].requiredClue != "" && !Notebook.Instance.IsClueDiscovered(dialogueStage.responses[i].requiredClue))
                continue;
            int j = i;

            //GameObject responseObject = GameObject.Instantiate(responsePrefab, transform) as GameObject;
            GameObject responseObject = GameObject.Instantiate(responsePrefab, responsesParent) as GameObject;
            responseObject.transform.localPosition = topResponsePosition + responseOffset * j;
            ClickableArea responseArea = responseObject.GetComponentInChildren<ClickableArea>();
            TextMeshPro textMesh = responseObject.GetComponentInChildren<TextMeshPro>();
            textMesh.text = dialogueStage.responses[j].response;

            
            if (dialogueStage.responses[j].nextStage == null)
            {
                responseArea.onClick.AddListener(() => {
                    // Analytics
                    AnalyticsManager.Instance.LogDialogueEvent(dialogueStage.treeName, dialogueStage.prompt, dialogueStage.responses[j].response);

                    CameraManager.Instance.GoToLast();
                    paperFromNotebookButton.SetActive(true);
                    phoneFromNotebookButton.SetActive(false);
                    Notebook.Instance.EnablePhonebook();
                });
            }
            else
            {
                responseArea.onClick.AddListener(() => {
                    // Analytics
                    AnalyticsManager.Instance.LogDialogueEvent(dialogueStage.treeName, dialogueStage.prompt, dialogueStage.responses[j].response);

                    TriggerDialogue(dialogueStage.responses[j].nextStage);
                });
            }
            
            responseArea.onClick.AddListener(() => {
                dialogueStage.responses[j].onContinue.Invoke();
            });
            //responseArea.onClick.AddListener(ResetResponses);

            responseAreas.Add(responseArea);
        }

        // Begin animation
        responsesParent.gameObject.SetActive(false);
        prompt.transform.parent.GetComponentInChildren<TypeTextBehaviour>().StartAnimation();

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