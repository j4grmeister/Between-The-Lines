using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    [SerializeField] private NewspaperPage[] pages;
    [SerializeField] private SpriteRenderer onePageBackground;
    [SerializeField] private SpriteRenderer twoPageBackground;
    
    private int pageIndex;

    void Start()
    {
        pageIndex = 0;
        if (pages.Length > 0)
        {
            
        }
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
}