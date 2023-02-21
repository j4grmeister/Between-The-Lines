using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewsArticle : MonoBehaviour
{
    [SerializeField] private string headline;
    [SerializeField] private string text;
    [SerializeField] private Sprite photo;
    [SerializeField] private TextMeshPro headlineText;

    public void Start()
    {
        headlineText.text = headline;
    }

    public void Expand()
    {
        NewsManager.Instance.SetCurrentArticle(this);
        NewsManager.Instance.Show();
    }

    public string GetHeadline()
    {
        return headline;
    }

    public string GetText()
    {
        return text;
    }

    public Sprite GetPhoto()
    {
        return photo;
    }
}