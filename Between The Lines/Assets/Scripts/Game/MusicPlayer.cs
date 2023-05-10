using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : Singleton<MusicPlayer>
{
    [SerializeField] private AudioClip defaultSong;
    [SerializeField] private AudioClip muffledSong;

    private AudioSource audioSource;
    private bool playingDefault;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource.clip = defaultSong;
        audioSource.Play();
        playingDefault = true;
    }

    void SwitchSong(AudioClip newSong)
    {
        float oldTime = audioSource.time;
        audioSource.clip = newSong;
        audioSource.time = oldTime;
        audioSource.Play();
    }

    public void PlayDefaultSong()
    {
        if (!playingDefault)
        {
            SwitchSong(defaultSong);
            playingDefault = true;
        }
    }

    public void PlayMuffledSong()
    {
        if (playingDefault)
        {
            SwitchSong(muffledSong);
            playingDefault = false;
        }
    }
}