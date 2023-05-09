using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TypeTextBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshPro Text;
    [SerializeField] private float typeDelay;
    [SerializeField] private UnityEvent onTypingFinished;

    private string fullText;
    private bool done = true;

    private float characterTimestamp;
    private int characterIndex;

    void Update()
    {
        if (!done)
        {
            if (Time.time - characterTimestamp >= typeDelay)
            {
                Text.text += fullText[characterIndex];
                characterIndex++;

                if (characterIndex >= fullText.Length)
                {
                    onTypingFinished.Invoke();
                    done = true;
                    return;
                } 

                characterTimestamp += typeDelay;
            }
        }
    }

    public void StartAnimation()
    {
        fullText = Text.text;
        Text.text = "";
        done = false;
        characterTimestamp = Time.time;
        characterIndex = 0;
    }

    public void EndAnimation()
    {
        Text.text = fullText;
        done = true;
        onTypingFinished.Invoke();
    }
}