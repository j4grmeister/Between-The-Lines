using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Vector2 notebookPosition;
    [SerializeField] private Vector2 phonePosition;

    private Vector2 paperPosition;

    void Start()
    {
        paperPosition = new Vector2(transform.position.x, transform.position.y);
    }

    public void MoveCamera(Vector2 targetPosition)
    {
        transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
    }

    public void GoToPaper()
    {
        MoveCamera(paperPosition);
    }

    public void GoToNotebook()
    {
        MoveCamera(notebookPosition);
    }

    public void GoToPhone()
    {
        MoveCamera(phonePosition);
    }
}