using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notebook : Singleton<Notebook>
{
    [SerializeField] private GameObject tableOfContentsParent;
    [SerializeField] private GameObject cluesParent;
    [SerializeField] private GameObject phonebookParent;

    [SerializeField] private GameObject phonePrefab;
    [SerializeField] private Vector2 topPhonePosition;
    [SerializeField] private Vector2 phoneOffset;
    [SerializeField] private int maxPhonesPerPage;

    private Hashtable clueTable = new Hashtable();
    private Hashtable phoneTable = new Hashtable();

    private Clue[] allClues;
    private PhoneNumber[] allPhones;

    private List<GameObject> phonePages = new List<GameObject>();
    private int pageIndex;

    void Start()
    {
        allClues = cluesParent.GetComponentsInChildren<Clue>(true);
        foreach (Clue c in allClues)
        {
            clueTable.Add(c.gameObject.name, c);
        }
        PhoneNumber[] allPhones = phonebookParent.GetComponentsInChildren<PhoneNumber>(true);
        foreach (PhoneNumber p in allPhones)
        {
            p.GetComponentInChildren<ClickableArea>().onClick.AddListener(() => {Call(p.gameObject.name);});
            phoneTable.Add(p.gameObject.name, p);
        }

        //initPhones();

        GameObject firstPhonePage = new GameObject("Page 1");
        firstPhonePage.transform.parent = phonebookParent.transform;
        firstPhonePage.transform.localPosition = Vector3.zero;
        firstPhonePage.SetActive(false);
        phonePages.Add(firstPhonePage);
    }

    // IMPORTANT: These do not handle non-existent names!!!

    public void DiscoverPhoneNumber(string phoneName)
    {   
        PhoneNumber phone = (PhoneNumber)phoneTable[phoneName];
        if (!phone.gameObject.activeSelf)
        {
            phone.gameObject.SetActive(true);

            int pageLength = phonePages[phonePages.Count-1].GetComponentsInChildren<PhoneNumber>().Length;
            if (pageLength >= maxPhonesPerPage)
            {
                GameObject nextPhonePage = new GameObject("Page " + phonePages.Count.ToString());
                nextPhonePage.transform.parent = phonebookParent.transform;
                nextPhonePage.transform.localPosition = Vector3.zero;
                nextPhonePage.SetActive(false);
                phonePages.Add(nextPhonePage);
                pageLength = 0;
            }
            phone.transform.parent = phonePages[phonePages.Count-1].transform;
            phone.transform.localPosition = (Vector3)(topPhonePosition + phoneOffset * pageLength) + Vector3.back * 3;
        }
    }

    public void Call(string phoneName)
    {
        DiscoverPhoneNumber(phoneName);
        ((PhoneNumber)phoneTable[phoneName]).Call();
    }

    public void DiscoverClue(string clueName)
    {
        ((Clue)clueTable[clueName]).gameObject.SetActive(true);
    }

    public bool IsClueDiscovered(string clueName)
    {
        return ((Clue)clueTable[clueName]).gameObject.activeSelf;
    }

    public void GoToContents()
    {
        tableOfContentsParent.SetActive(true);
        cluesParent.SetActive(false);
        phonebookParent.SetActive(false);

        SetPage(0);
    }

    public void GoToClues()
    {
        tableOfContentsParent.SetActive(false);
        cluesParent.SetActive(true);
        phonebookParent.SetActive(false);

        SetPage(0);
    }

    public void GoToPhonebook()
    {
        tableOfContentsParent.SetActive(false);
        cluesParent.SetActive(false);
        phonebookParent.SetActive(true);

        SetPage(0);
    }

    void SetPage(int pageIndex)
    {
        if (phonebookParent.activeSelf)
        {
            phonePages[this.pageIndex].SetActive(false);
            phonePages[pageIndex].SetActive(true);
        }
        this.pageIndex = pageIndex;
    }



    // Deprecated
    void initPhones()
    {
        foreach(PhoneNumber p in allPhones)
        {
            GameObject prefabInstance = GameObject.Instantiate(phonePrefab, p.transform);
            prefabInstance.GetComponentInChildren<ClickableArea>().onClick.AddListener(() => {Call(p.gameObject.name);});
            prefabInstance.GetComponentInChildren<TextMeshPro>().text = p.gameObject.name;
        }
    }
}