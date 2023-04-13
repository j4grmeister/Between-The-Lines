using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class AnalyticsManager : Singleton<AnalyticsManager>
{
    async void Awake()
	{
		try
		{
			await UnityServices.InitializeAsync();
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}

    public void LogDialogueEvent(string treeName, string prompt, string response)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            {"dialogueTreeName", treeName},
            {"dialoguePrompt", prompt},
            {"dialogueResponse", response},
        };

        AnalyticsService.Instance.CustomData("dialoguePicked", parameters); 
    }
}