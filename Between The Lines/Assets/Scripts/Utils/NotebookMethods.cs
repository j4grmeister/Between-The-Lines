using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This MonoBehaviour can be attached to any object to allow UnityEvents
// on that object to be able to access singleton class methods

public class NotebookMethods : MonoBehaviour
{
    [System.NonSerialized] public Notebook Notebook;

    void Awake()
    {
        Notebook = Notebook.Instance;
    }

    public void Call(string phoneName)
    {
        Notebook.Call(phoneName);
    }

    public void DiscoverPhoneNumber(string phoneName)
    {
        Notebook.DiscoverClue(phoneName);
    }

    public void DiscoverClue(string clueName)
    {
        Notebook.DiscoverClue(clueName);
    }
}