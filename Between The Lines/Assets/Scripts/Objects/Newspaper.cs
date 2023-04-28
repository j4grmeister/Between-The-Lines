using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Newspaper : MonoBehaviour
{
    [System.Serializable]
    public enum PageType
    {
        ONEPAGE,
        TWOPAGE
    }

    [System.Serializable]
    public struct NewspaperPage
    {
        public GameObject pageObject;
        public PageType pageType;
        public UnityEvent onFirstView;
    }


    [SerializeField] private GameObject nextPageButtonOnePage;
    [SerializeField] private GameObject previousPageButtonOnePage;
    [SerializeField] private GameObject nextPageButtonTwoPage;
    [SerializeField] private GameObject previousPageButtonTwoPage;
    [SerializeField] private NewspaperPage[] pages;
    [SerializeField] private SpriteRenderer onePageBackground;
    [SerializeField] private SpriteRenderer twoPageBackground;

    private bool[] pagesViewed;
    
    private int pageIndex;

    void Start()
    {
        pagesViewed = new bool[pages.Length];

        pageIndex = 0;
        previousPageButtonOnePage.SetActive(false);
        previousPageButtonTwoPage.SetActive(false);
        if (pages.Length > 0)
        {
            pages[0].pageObject.SetActive(true);
            for(int i = 1; i < pages.Length; i++)
            {
                pages[i].pageObject.SetActive(false);
            }
        }
        if (pages.Length > 1)
        {
            nextPageButtonOnePage.SetActive(true);
            nextPageButtonTwoPage.SetActive(true);
        }
        else
        {
            nextPageButtonOnePage.SetActive(false);
            nextPageButtonTwoPage.SetActive(false);
        }

        SetBackground(pages[0].pageType);
    }

    void SetBackground(PageType pageType)
    {
        switch (pageType)
        {
            case PageType.ONEPAGE:
                onePageBackground.gameObject.SetActive(true);
                twoPageBackground.gameObject.SetActive(false);
                WatchManager.Instance.OnePage();
                break;
            case PageType.TWOPAGE:
                onePageBackground.gameObject.SetActive(false);
                twoPageBackground.gameObject.SetActive(true);
                WatchManager.Instance.TwoPage();
                break;
        }
    }

    public void NextPage()
    {
        pages[pageIndex].pageObject.SetActive(false);

        if (pageIndex == 0)
        {
            previousPageButtonOnePage.SetActive(true);
            previousPageButtonTwoPage.SetActive(true);
        }

        pageIndex++;
        pages[pageIndex].pageObject.SetActive(true);

        if (pageIndex == pages.Length - 1)
        {
            nextPageButtonOnePage.SetActive(false);
            nextPageButtonTwoPage.SetActive(false);
        }

        SetBackground(pages[pageIndex].pageType);

        if (!pagesViewed[pageIndex])
        {
            pages[pageIndex].onFirstView.Invoke();
            pagesViewed[pageIndex] = true;
        }
    }

    public void PreviousPage()
    {
        pages[pageIndex].pageObject.SetActive(false);

        if (pageIndex == pages.Length - 1)
        {
            nextPageButtonOnePage.SetActive(true);
            nextPageButtonTwoPage.SetActive(true);
        }

        pageIndex--;
        pages[pageIndex].pageObject.SetActive(true);

        if (pageIndex == 0)
        {
            previousPageButtonOnePage.SetActive(false);
            previousPageButtonTwoPage.SetActive(false);
        }

        SetBackground(pages[pageIndex].pageType);
    }
}