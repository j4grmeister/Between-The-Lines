using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallInterupt : MonoBehaviour
{
    [SerializeField] public AudioClip ringClip;
    [SerializeField] public int interuptTurnNumber;
    [SerializeField] public string phoneName;
    [SerializeField] public FrameAnimator animation;
    public bool discover = true;

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
            if (discover)
            {
                //Notebook.Instance.Call(phoneName);
                if (animation != null)
                {
                    animation.onFinish = () =>
                    {
                        Notebook.Instance.Call(phoneName);
                    };
                    animation.Play();
                }
                else {
                    Notebook.Instance.Call(phoneName);
                }
            }
            else
            {
                //Notebook.Instance.CallNoDiscover(phoneName);
                if (animation != null)
                {
                    animation.onFinish = () =>
                    {
                        Notebook.Instance.CallNoDiscover(phoneName);
                    };
                    animation.Play();
                }
                else {
                    Notebook.Instance.CallNoDiscover(phoneName);
                }
            }
            });
    }
}