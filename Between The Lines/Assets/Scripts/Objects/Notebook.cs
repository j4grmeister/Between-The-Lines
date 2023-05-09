using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notebook : Singleton<Notebook>
{
    [SerializeField] private GameObject tableOfContentsParent;
    [SerializeField] private GameObject cluesParent;
    [SerializeField] private GameObject phonebookParent;

    [SerializeField] private GameObject nextPageButton;
    [SerializeField] private GameObject previousPageButton;

    [SerializeField] private GameObject phonePrefab;
    [SerializeField] private Vector2 topPhonePosition;
    [SerializeField] private Vector2 phoneOffset;
    [SerializeField] private int maxPhonesPerPage;

    [SerializeField] private Vector2 topCluePosition;
    [SerializeField] private Vector2 clueOffset;
    [SerializeField] private int maxCluesPerPage;
    [SerializeField] private GameObject expandedClueObject;
    [SerializeField] private TextMeshPro expandedClueTitle;
    [SerializeField] private TextMeshPro expandedClueDescription;

    private Hashtable clueTable = new Hashtable();
    private Hashtable phoneTable = new Hashtable();


    [SerializeField] private TextMeshPro phonebookText;
    private Color phonebookColor;
    [SerializeField] private Color disabledColor;
    private Clue[] allClues;
    private PhoneNumber[] allPhones;

    private List<GameObject> phonePages = new List<GameObject>();
    private List<GameObject> cluePages = new List<GameObject>();
    private int pageIndex;

    void Awake()
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

        GameObject firstCluePage = new GameObject("Page 1");
        firstCluePage.transform.parent = cluesParent.transform;
        firstCluePage.transform.localPosition = Vector3.zero;
        firstCluePage.SetActive(false);
        cluePages.Add(firstCluePage);

        DiscoverPhoneNumberNoNotification("BPD");
    }

    void Start()
    {
        phonebookColor = phonebookText.color;
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
            NotificationManager.Instance.NotifyPhoneNumber(phone);
        }
    }

    public void DiscoverPhoneNumberNoNotification(string phoneName)
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

    public void UndiscoverPhoneNumber(string phoneName)
    {
        // TODO:
    }

    public void RevealPhoneName(string phoneName)
    {
        PhoneNumber phone = (PhoneNumber)phoneTable[phoneName];
        phone.SetName(phone.gameObject.name);
    }

    public string GetVisiblePhoneName(string phoneName)
    {
        return ((PhoneNumber)phoneTable[phoneName]).GetVisibleName();
    }

    public void Call(string phoneName)
    {
        DiscoverPhoneNumber(phoneName);
        ((PhoneNumber)phoneTable[phoneName]).Call();
    }
    
    public void CallNoDiscover(string phoneName)
    {
        ((PhoneNumber)phoneTable[phoneName]).Call();
    }

    public void DiscoverClue(string clueName)
    {
        Clue clue = (Clue)clueTable[clueName];
        if (!clue.gameObject.activeSelf)
        {
            clue.gameObject.SetActive(true);

            int pageLength = cluePages[cluePages.Count-1].GetComponentsInChildren<Clue>().Length;
            if (pageLength >= maxCluesPerPage)
            {
                GameObject nextCluePage = new GameObject("Page " + cluePages.Count.ToString());
                nextCluePage.transform.parent = cluesParent.transform;
                nextCluePage.transform.localPosition = Vector3.zero;
                nextCluePage.SetActive(false);
                cluePages.Add(nextCluePage);
                pageLength = 0;
            }
            clue.transform.parent = cluePages[cluePages.Count-1].transform;
            clue.transform.localPosition = (Vector3)(topCluePosition + clueOffset * pageLength) + Vector3.back * 3;

            NotificationManager.Instance.NotifyClue(clue);
        }
    }

    public void UndiscoverClue(string clueName)
    {
        // TODO
    }

    public void ChangeDialogueTree(string phoneName, string treeName)
    {
        // TODO
    }

    public bool IsClueDiscovered(string clueName)
    {
        if (clueTable[clueName] == null)
        {
            Debug.Log("ERROR: missing clue " + clueName);
            return false;
        }
        return ((Clue)clueTable[clueName]).gameObject.activeSelf;
    }

    public void GoToContents()
    {
        tableOfContentsParent.SetActive(true);
        cluesParent.SetActive(false);
        phonebookParent.SetActive(false);

        nextPageButton.SetActive(false);
        previousPageButton.SetActive(false);

        SetPage(0);

        SoundEffectManager.Instance.PlayNotebookFlip();
    }

    public void GoToClues()
    {
        tableOfContentsParent.SetActive(false);
        cluesParent.SetActive(true);
        phonebookParent.SetActive(false);

        SetPage(0);
        SoundEffectManager.Instance.PlayNotebookFlip();
    }

    public void GoToPhonebook()
    {
        tableOfContentsParent.SetActive(false);
        cluesParent.SetActive(false);
        phonebookParent.SetActive(true);

        SetPage(0);
        SoundEffectManager.Instance.PlayNotebookFlip();
    }

    public void GoToPhonebookQuietly()
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

            if (pageIndex == 0)
            {
                previousPageButton.SetActive(false);
                if (phonePages.Count > 1)
                {
                    nextPageButton.SetActive(true);
                }
                else
                {
                    nextPageButton.SetActive(false);
                }
            }
            else if (pageIndex == phonePages.Count - 1)
            {
                previousPageButton.SetActive(true);
                nextPageButton.SetActive(false);
            }
            else
            {
                previousPageButton.SetActive(true);
                nextPageButton.SetActive(true);
            }
        }
        else if (cluesParent.activeSelf)
        {
            cluePages[this.pageIndex].SetActive(false);
            cluePages[pageIndex].SetActive(true);

            if (pageIndex == 0)
            {
                previousPageButton.SetActive(false);
                if (cluePages.Count > 1)
                {
                    nextPageButton.SetActive(true);
                }
                else
                {
                    nextPageButton.SetActive(false);
                }
            }
            else if (pageIndex == cluePages.Count - 1)
            {
                previousPageButton.SetActive(true);
                nextPageButton.SetActive(false);
            }
            else
            {
                previousPageButton.SetActive(true);
                nextPageButton.SetActive(true);
            }
        }
        this.pageIndex = pageIndex;
    }

    public void NextPage()
    {
        SetPage(pageIndex + 1);
        SoundEffectManager.Instance.PlayNotebookFlip();
    }

    public void PreviousPage()
    {
        SetPage(pageIndex - 1);
        SoundEffectManager.Instance.PlayNotebookFlip();
    }

    public void ExpandClue(Clue clue)
    {
        expandedClueTitle.text = clue.GetName();
        expandedClueDescription.text = clue.GetDescription();
        expandedClueObject.SetActive(true);
        SoundEffectManager.Instance.PlayNotebookFlip();
    }

    public void ShrinkClue()
    {
        expandedClueObject.SetActive(false);
        SoundEffectManager.Instance.PlayNotebookFlip();
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

    public void DisablePhonebook()
    {
        GoToContents();
        phonebookText.color = disabledColor;
        phonebookText.transform.parent.GetComponentInChildren<Collider2D>(true).gameObject.SetActive(false);
    }

    public void EnablePhonebook()
    {
        phonebookText.color = phonebookColor;
        phonebookText.transform.parent.GetComponentInChildren<Collider2D>(true).gameObject.SetActive(true);
    }
}