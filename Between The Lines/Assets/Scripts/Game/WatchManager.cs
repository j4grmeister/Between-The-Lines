using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchManager : Singleton<WatchManager>
{
    
    [SerializeField] private Vector2 onePagePosition;
    [SerializeField] private Vector2 twoPagePosition;

    [SerializeField] private SpriteRenderer hourHandRenderer;
    [SerializeField] private SpriteRenderer minuteHandRenderer;

    [SerializeField] private Sprite[] hourSprites;
    [SerializeField] private Sprite[] minuteSprites;

    [SerializeField] private int maxTurns;

    [System.NonSerialized]
    public int turnNumber;

    void Start()
    {
        minuteHandRenderer.sprite = minuteSprites[0];
        Reset();
    }

    public void Reset()
    {
        SetTurn(0);
    }

    public void SetTurn(int turnNumber)
    {
        this.turnNumber = turnNumber % maxTurns;
        hourHandRenderer.sprite = hourSprites[Mathf.FloorToInt((float)this.turnNumber / (float)maxTurns * hourSprites.Length)];
    }

    public void NextTurn()
    {
        if (turnNumber == maxTurns-1)
        {
            Reset();
            NewsManager.Instance.NextLevel();
        }
        else
        {
            SetTurn(turnNumber+1);
        }
    }

    public void OnePage()
    {
        transform.localPosition = new Vector3(onePagePosition.x, onePagePosition.y, transform.localPosition.z);
    }

    public void TwoPage()
    {
        transform.localPosition = new Vector3(twoPagePosition.x, twoPagePosition.y, transform.localPosition.z);
    }
}