using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : MonoBehaviour
{
    [SerializeField] private string description;

    public string GetName()
    {
        return gameObject.name;
    }

    public string GetDescription()
    {
        return description;
    }

    public void Expand()
    {
        Notebook.Instance.ExpandClue(this);
    }
}