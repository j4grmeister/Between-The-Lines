using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Clue : MonoBehaviour
{
    [SerializeField] private TextMeshPro name;
    [SerializeField] private string description;

    public string GetName()
    {
        //return gameObject.name;
        return name.text;
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