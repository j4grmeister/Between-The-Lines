using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkTextColorBehaviour : MonoBehaviour
{
    [SerializeField] private Color blinkColor;
    [SerializeField] private float blinkInterval;

    private Color originalColor;

    private TextMeshPro tmpro;

    private float blinkTimestamp;

    void Awake()
    {
        tmpro = GetComponent<TextMeshPro>();
    }

    void Start()
    {
        originalColor = tmpro.color;
        blinkTimestamp = Time.time;
    }

    void Update()
    {
        if (Time.time - blinkTimestamp >= blinkInterval)
        {
            if (Time.time - blinkTimestamp >= blinkInterval * 2)
            {
                tmpro.color = originalColor;
                blinkTimestamp = Time.time;
            }
            else if (tmpro.color != blinkColor)
            {
                tmpro.color = blinkColor;
            }
        }
    }

    void OnDisable()
    {
        tmpro.color = originalColor;
    }
}