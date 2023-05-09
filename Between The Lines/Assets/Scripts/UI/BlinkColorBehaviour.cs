using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkColorBehaviour : MonoBehaviour
{
    [SerializeField] private Color blinkColor;
    [SerializeField] private float blinkInterval;

    private Color originalColor;

    private SpriteRenderer spriteRenderer;

    private float blinkTimestamp;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        originalColor = spriteRenderer.color;
        blinkTimestamp = Time.time;
    }

    void Update()
    {
        if (Time.time - blinkTimestamp >= blinkInterval)
        {
            if (Time.time - blinkTimestamp >= blinkInterval * 2)
            {
                spriteRenderer.color = originalColor;
                blinkTimestamp = Time.time;
            }
            else if (spriteRenderer.color != blinkColor)
            {
                spriteRenderer.color = blinkColor;
            }
        }
    }

    void OnDisable()
    {
        spriteRenderer.color = originalColor;
    }
}