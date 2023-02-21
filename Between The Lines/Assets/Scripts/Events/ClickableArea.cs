using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class ClickableArea : MonoBehaviour
{
    [SerializeField] private UnityEvent onClick;

    private Collider2D clickableArea;

    void Awake()
    {
        clickableArea = GetComponent<Collider2D>();
    }

    void Start()
    {

    }

    public void Invoke()
    {
        onClick.Invoke();
    }
}