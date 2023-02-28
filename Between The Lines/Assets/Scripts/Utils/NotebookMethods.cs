using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This MonoBehaviour can be attached to any object to allow UnityEvents
// on that object to be able to access singleton class methods

public class NotebookMethods : MonoBehaviour
{
    public void Call(string phoneName)
    {
        Notebook.Instance.Call(phoneName);
    }

    public void DiscoverPhoneNumber(string phoneName)
    {
        Notebook.Instance.DiscoverPhoneNumber(phoneName);
    }

    public void DiscoverClue(string clueName)
    {
        Notebook.Instance.DiscoverClue(clueName);
    }
}