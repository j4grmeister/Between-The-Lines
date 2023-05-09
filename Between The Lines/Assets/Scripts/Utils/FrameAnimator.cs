using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public bool playBackwards;

    public UnityAction onFinish;
    [SerializeField] private UnityEvent onAnimationFinished;

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
            int numberOfFrames = playBackwards ? animationFrames[animationFrames.Length - index - 1].numberOfFrames : animationFrames[index].numberOfFrames;
            //if (nestIndex >= animationFrames[index].numberOfFrames)
            if (nestIndex >= numberOfFrames)
            {
                nestIndex = 0;
                index++;
                if (index >= animationFrames.Length)
                {
                    gameObject.SetActive(false);
                    onAnimationFinished.Invoke();
                    if (onFinish != null)
                    {
                        onFinish();
                        onFinish = null;
                    }
                    return;
                }
                //spriteRenderer.sprite = animationFrames[index].sprite;
                spriteRenderer.sprite = playBackwards ? animationFrames[animationFrames.Length - index - 1].sprite : animationFrames[index].sprite;
            }
        }
    }

    public void Play()
    {
        gameObject.SetActive(true);
        index = 0;
        nestIndex = 0;
        lastFrame = 0f;

        if (spriteRenderer == null)
        {
            Awake();
        }

        //spriteRenderer.sprite = animationFrames[0].sprite;
        spriteRenderer.sprite = playBackwards ? animationFrames[animationFrames.Length - 1].sprite : animationFrames[0].sprite;
    }
}