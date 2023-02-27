using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notebook : Singleton<Notebook>
{
    [SerializeField] private GameObject cluesParent;
    [SerializeField] private GameObject phonebookParent;

    private Hashtable allClues = new Hashtable();
    private Hashtable allPhones = new Hashtable();

    void Start()
    {
        Clue[] clueList = cluesParent.GetComponentsInChildren<Clue>(true);
        foreach (Clue c in clueList)
        {
            allClues.Add(c.gameObject.name, c);
        }
        PhoneNumber[] phoneList = phonebookParent.GetComponentsInChildren<PhoneNumber>(true);
        foreach (PhoneNumber p in phoneList)
        {
            allPhones.Add(p.gameObject.name, p);
        }
    }

    // IMPORTANT: These do not handle non-existent names!!!

    public void DiscoverPhoneNumber(string phoneName)
    {
        ((PhoneNumber)allPhones[phoneName]).gameObject.SetActive(true);
    }

    public void Call(string phoneName)
    {
        ((PhoneNumber)allPhones[phoneName]).Call();
    }

    public void DiscoverClue(string clueName)
    {
        ((Clue)allClues[clueName]).gameObject.SetActive(true);
    }

    public bool IsClueDiscovered(string clueName)
    {
        return ((Clue)allClues[clueName]).gameObject.activeSelf;
    }
}