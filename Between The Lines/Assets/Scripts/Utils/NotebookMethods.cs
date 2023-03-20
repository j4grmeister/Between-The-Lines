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

    public void UndiscoverPhoneNumber(string phoneName)
    {
        Notebook.Instance.UndiscoverPhoneNumber(phoneName);
    }

    public void DiscoverClue(string clueName)
    {
        Notebook.Instance.DiscoverClue(clueName);
    }

    public void UndiscoverClue(string clueName)
    {
        Notebook.Instance.UndiscoverClue(clueName);
    }

    public void RevealPhoneName(string phoneName)
    {
        Notebook.Instance.RevealPhoneName(phoneName);
    }

    public void ChangeDialogueTree(string args)
    {
        string[] split = args.Split(',');
        Notebook.Instance.ChangeDialogueTree(split[0], split[1]);
    }
}