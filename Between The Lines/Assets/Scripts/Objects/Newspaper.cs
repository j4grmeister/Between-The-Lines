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


    [SerializeField] private GameObject nextPageButton;
    [SerializeField] private GameObject previousPageButton;
    [SerializeField] private NewspaperPage[] pages;
    [SerializeField] private SpriteRenderer onePageBackground;
    [SerializeField] private SpriteRenderer twoPageBackground;

    private bool[] pagesViewed;
    
    private int pageIndex;

    void Start()
    {
        pagesViewed = new bool[pages.Length];

        pageIndex = 0;
        previousPageButton.SetActive(false);
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
            nextPageButton.SetActive(true);
        }
        else
        {
            nextPageButton.SetActive(false);
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
                break;
            case PageType.TWOPAGE:
                onePageBackground.gameObject.SetActive(false);
                twoPageBackground.gameObject.SetActive(true);
                break;
        }
    }

    public void NextPage()
    {
        pages[pageIndex].pageObject.SetActive(false);

        if (pageIndex == 0)
        {
            previousPageButton.SetActive(true);
        }

        pageIndex++;
        pages[pageIndex].pageObject.SetActive(true);

        if (pageIndex == pages.Length - 1)
        {
            nextPageButton.SetActive(false);
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
            nextPageButton.SetActive(true);
        }

        pageIndex--;
        pages[pageIndex].pageObject.SetActive(true);

        if (pageIndex == 0)
        {
            previousPageButton.SetActive(false);
        }

        SetBackground(pages[pageIndex].pageType);
    }
}