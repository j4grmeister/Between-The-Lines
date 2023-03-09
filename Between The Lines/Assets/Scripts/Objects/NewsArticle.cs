using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class NewsArticle : MonoBehaviour
{
    [SerializeField] private string headline;
    [SerializeField] private string text;
    [SerializeField] private Sprite photo;
    [SerializeField] private TextMeshPro headlineText;
    [SerializeField] private GameObject expandedVersion;

    [SerializeField] private UnityEvent onFirstExpand;

    private bool openedAlready = false;

    public void Start()
    {
        //headlineText.text = headline;
    }

    public void Expand()
    {
        NewsManager.Instance.SetCurrentArticle(this); // TODO: Deprecate this?
        NewsManager.Instance.Show();
        Vector3 pos = NewsManager.Instance.expandedArticle.transform.position;
        expandedVersion.transform.position = new Vector3(pos.x, pos.y, expandedVersion.transform.position.z);
        expandedVersion.SetActive(true);
        if (!openedAlready)
        {
            onFirstExpand.Invoke();

            openedAlready = true;
        }
    }

    public void Shrink()
    {
        expandedVersion.SetActive(false);
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