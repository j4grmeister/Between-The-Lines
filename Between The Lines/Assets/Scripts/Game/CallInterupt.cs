using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallInterupt : MonoBehaviour
{
    [SerializeField] public AudioClip ringClip;
    [SerializeField] public int interuptTurnNumber;
    [SerializeField] private string phoneName;

    private bool triggered = false;

    private int lastTurnNumber;

    void Start()
    {
        lastTurnNumber = WatchManager.Instance.turnNumber;
    }

    void Update()
    {
        if (lastTurnNumber > WatchManager.Instance.turnNumber)
        {
            gameObject.SetActive(false);
        }
        if (WatchManager.Instance.turnNumber >= interuptTurnNumber && CameraManager.Instance.transform.position.x == CameraManager.Instance.paperPosition.x && CameraManager.Instance.transform.position.y == CameraManager.Instance.paperPosition.y)
        {
            enabled = false;
            Interupt();
        }
    }

    void Interupt()
    {
        //Notebook.Instance.Call(phoneName);
        SoundEffectManager.Instance.DoActionAfterPlay(ringClip, () => {
            WatchManager.Instance.turnNumber--;
            Notebook.Instance.Call(phoneName);
            });
    }
}