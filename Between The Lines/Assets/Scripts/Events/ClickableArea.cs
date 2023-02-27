using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class ClickableArea : MonoBehaviour
{
    public UnityEvent onClick;

    private Collider2D clickableArea;

    void Awake()
    {
        clickableArea = GetComponent<Collider2D>();
    }

    public void Invoke()
    {
        onClick.Invoke();
    }
}