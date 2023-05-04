using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnClueDiscovered : MonoBehaviour
{
    [SerializeField] private string clueName;
    [SerializeField] private UnityEvent onClueDiscovered;

    void Update()
    {
        if (Notebook.Instance.IsClueDiscovered(clueName))
        {
            onClueDiscovered.Invoke();
            gameObject.SetActive(false);
        }
    }
}