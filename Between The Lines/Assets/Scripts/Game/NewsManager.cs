using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewsManager : Singleton<NewsManager>
{
    [SerializeField] private GameObject expandedStory;
    [SerializeField] private TextMeshPro articleHeadline;
    [SerializeField] private TextMeshPro articleText;
    [SerializeField] private SpriteRenderer articlePhoto;

    private NewsArticle currentArticle;

    private NewsArticle[] savedArticles;

    void Start()
    {
        
    }

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
        expandedStory.SetActive(true);
    }
}