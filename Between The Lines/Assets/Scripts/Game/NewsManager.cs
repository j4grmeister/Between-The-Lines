using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewsManager : Singleton<NewsManager>
{
    [SerializeField] private int maxSavedArticles = 3;
    [SerializeField] private List<GameObject> levels;
    [SerializeField] private List<FrameAnimator> endOfDayAnimators;

    [SerializeField] private GameObject levelParent;
    [SerializeField] private GameObject savedArticlesParent;

    public GameObject expandedArticle;
    [SerializeField] private TextMeshPro articleHeadline;
    [SerializeField] private TextMeshPro articleText;
    [SerializeField] private SpriteRenderer articlePhoto;

    [SerializeField] private FrameAnimator[] animations;

    [SerializeField] private GameObject winParent;
    [SerializeField] private GameObject winAltParent;
    [SerializeField] private GameObject loseParent;

    [SerializeField] private AudioClip knockingClip;

    [SerializeField] private FrameAnimator pizzaAnimation;

    public GameObject notebookButton1;
    public GameObject notebookButton2;


    private NewsArticle currentArticle;

    private NewsArticle[] savedArticles;

    private GameObject currentLevel;
    private int levelIndex;

    void Awake()
    {
        savedArticles = new NewsArticle[maxSavedArticles];

        levelIndex = 0;
        if (levels.Count > 0)
        {
            currentLevel = GameObject.Instantiate(levels[0], levelParent.transform);
        }
    }

    void Start()
    {
        WatchManager.Instance.turnNumber--;
        //Notebook.Instance.Call("BPD");
        Notebook.Instance.DiscoverPhoneNumberNoNotification("BPD");

        // Tutorial
        CameraManager.Instance.GoToNotebook();
        //Notebook.Instance.GoToPhonebookQuietly();
        Notebook.Instance.DiscoverClueNoNotification("introduction");
    }

    // TODO: Deprecate this?
    public void SetCurrentArticle(NewsArticle article)
    {
        currentArticle = article;
        articleHeadline.text = currentArticle.GetHeadline();
        articleText.text = currentArticle.GetText();
        Sprite photo = currentArticle.GetPhoto();
        if (photo != null) articlePhoto.sprite = photo;
    }

    public void Show()
    {
        expandedArticle.SetActive(true);
        //SavedArticleCounter.Instance.gameObject.SetActive(true);
    }

    public void Hide()
    {
        expandedArticle.SetActive(false);
        //SavedArticleCounter.Instance.gameObject.SetActive(false);
        currentArticle.Shrink();
    }

    public void SaveCurrentArticle()
    {
        int articleCount = SavedArticleCounter.Instance.GetSavedArticlesCount();
        if (articleCount < maxSavedArticles)
        {
            bool alreadySaved = false;
            int emptyIndex = -1;
            for (int i = 0; i < maxSavedArticles; i++)
            {
                if (savedArticles[i] == null)
                {
                    if (!alreadySaved)
                    {
                        emptyIndex = i;
                    }
                } 
                else if (savedArticles[i] == currentArticle)
                {
                    alreadySaved = true;
                }
            }

            if (!alreadySaved)
            {
                savedArticles[emptyIndex] = currentArticle;
                SavedArticleCounter.Instance.SetSavedArticlesCount(articleCount + 1);
            }
        }
    }

    public void NextLevel()
    {
        levelIndex++;
        //MoveSavedArticles();
        if (levelIndex < levels.Count)
        {
            GameObject.Destroy(currentLevel);
            currentLevel = GameObject.Instantiate(levels[levelIndex], levelParent.transform);
            animations[levelIndex-1].Play();
        }
        else
        {
            Lose();
        }
    }

    private void MoveSavedArticles()
    {
        for (int i = 0; i < savedArticles.Length; i++)
        {
            savedArticles[i].transform.parent = savedArticlesParent.transform;
            savedArticles[i] = null;
        }
        SavedArticleCounter.Instance.SetSavedArticlesCount(0);
    }

    public void Win()
    {
        CameraManager.Instance.GoToPaper();
        winParent.SetActive(true);
    }

    public void WinAlt()
    {
        CameraManager.Instance.GoToPaper();
        winAltParent.SetActive(true);
    }

    public void Lose()
    {
        CameraManager.Instance.GoToPaper();
        loseParent.SetActive(true);
    }
    
    public void OrderPizza()
    {
        GameObject callGO = new GameObject();
        CallInterupt call = callGO.AddComponent<CallInterupt>();
        call.ringClip = knockingClip;
        call.phoneName = "Pizza Order";
        call.discover = false;
        call.interuptTurnNumber = WatchManager.Instance.turnNumber + 1;
        call.animation = pizzaAnimation;
        if (call.interuptTurnNumber == 3) // THIS IS HARDCODED TO NOT INTERFERE WITH THE NEIGHBOR CALL
        {
            call.interuptTurnNumber++;
        }
    }
}