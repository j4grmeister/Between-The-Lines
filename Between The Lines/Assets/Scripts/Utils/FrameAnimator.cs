using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FrameAnimator : MonoBehaviour
{
    [System.Serializable]
    public struct FrameInfo
    {
        public Sprite sprite;
        public int numberOfFrames;
    }

    [SerializeField] private FrameInfo[] animationFrames;
    [SerializeField] private int framerate;

    private SpriteRenderer spriteRenderer;

    private int index;
    private int nestIndex;
    private float lastFrame;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        index = 0;
        nestIndex = 0;
        lastFrame = 0f;
        spriteRenderer.sprite = animationFrames[0].sprite;
    }

    void Update()
    {
        lastFrame += Time.deltaTime;
        if (lastFrame >= 1f/framerate)
        {
            lastFrame -= 1f/framerate;
            nestIndex++;
            if (nestIndex >= animationFrames[index].numberOfFrames)
            {
                nestIndex = 0;
                index++;
                if (index >= animationFrames.Length)
                {
                    gameObject.SetActive(false);
                    return;
                }
                spriteRenderer.sprite = animationFrames[index].sprite;
            }
        }
    }

    public void Play()
    {
        gameObject.SetActive(true);
        index = 0;
        nestIndex = 0;
        lastFrame = 0f;

        spriteRenderer.sprite = animationFrames[0].sprite;
    }
}