using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedArticleCounter : Singleton<SavedArticleCounter>
{
    [SerializeField] private GameObject[] savedArticleIcons;

    private int savedArticlesCount = 0;
    
    public int GetSavedArticlesCount()
    {
        return savedArticlesCount;
    }

    public void SetSavedArticlesCount(int savedArticlesCount)
    {
        this.savedArticlesCount = Math.Min(savedArticlesCount, savedArticleIcons.Length);
        for(int i = 0; i < savedArticlesCount; i++)
        {
            savedArticleIcons[i].SetActive(true);
        }
        for(int i = savedArticlesCount; i < savedArticleIcons.Length; i++)
        {
            savedArticleIcons[i].SetActive(false);
        }
    }
}