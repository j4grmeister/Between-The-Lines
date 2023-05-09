using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Vector2 notebookPosition;
    [SerializeField] private Vector2 phonePosition;

    [SerializeField] private GameObject animationsParent;

    [System.NonSerialized] public Vector2 paperPosition;

    private Vector2 lastPosition;

    void Start()
    {
        paperPosition = new Vector2(transform.position.x, transform.position.y);
        lastPosition = paperPosition;
    }

    public void MoveCamera(Vector2 targetPosition)
    {
        lastPosition = transform.position;
        transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
    }

    public void GoToPaper()
    {
        MoveCamera(paperPosition);
        animationsParent.SetActive(true);
        MusicPlayer.Instance?.PlayDefaultSong();
    }

    public void GoToNotebook()
    {
        MoveCamera(notebookPosition);
        animationsParent.SetActive(false);
        MusicPlayer.Instance?.PlayDefaultSong();
    }

    public void GoToPhone()
    {
        MoveCamera(phonePosition);
        animationsParent.SetActive(false);
        MusicPlayer.Instance?.PlayMuffledSong();
    }

    public void GoToLast()
    {
        if (lastPosition == paperPosition)
        {
            animationsParent.SetActive(true);
        }
        MoveCamera(lastPosition);
    }
}