using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// This entire class is basically just a wrapper
// These GameObjects can probably all be set to inactive,
// I figure that will make it easier on the engine
public class DialogueStage : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueResponse
    {
        public string response;
        public string requiredClue;
        public DialogueStage nextStage;
        public UnityEvent onContinue;
    }

    public string prompt;
    public DialogueResponse[] responses;

    [System.NonSerialized] public string linkedFilename;
}