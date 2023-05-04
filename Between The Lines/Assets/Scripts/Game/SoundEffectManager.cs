using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundEffectManager : Singleton<SoundEffectManager>
{
    [SerializeField] private Collider2D clickHider;

    [SerializeField] private AudioClip paperFlip;
    [SerializeField] private AudioClip notebookFlip;

    private AudioSource audioSource;

    private UnityAction nextAction;

    private bool actionWaiting = false;
    private float startTimestamp;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (actionWaiting && Time.time - startTimestamp >= audioSource.clip.length)
        {
            nextAction();
            actionWaiting = false;
            clickHider.gameObject.SetActive(false);
        }
    }

    public void DoActionAfterPlay(AudioClip audioClip, UnityAction action)
    {
        actionWaiting = true;
        nextAction = action;
        audioSource.clip = audioClip;
        startTimestamp = Time.time;
        audioSource.Play();
        clickHider.gameObject.SetActive(true);
    }

    public void PlayPaperFlip()
    {
        audioSource.clip = paperFlip;
        audioSource.Play();
    }

    public void PlayNotebookFlip()
    {
        audioSource.clip = notebookFlip;
        audioSource.Play();
    }
}