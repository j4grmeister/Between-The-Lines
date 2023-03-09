using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationManager : Singleton<NotificationManager>
{
    [SerializeField] private Button notificationButton;
    [SerializeField] private TextMeshProUGUI notificationTitle;
    [SerializeField] private TextMeshProUGUI notificationDescription;

    [SerializeField] private float notificationDisplayTime;


    float notificationTimestamp;

    void Update()
    {
        if (notificationButton.gameObject.activeSelf)
        {
            if (Time.time - notificationTimestamp >= notificationDisplayTime)
            {
                notificationButton.gameObject.SetActive(false);
            }
        }
    }

    public void NotifyPhoneNumber(PhoneNumber phone)
    {
        notificationTimestamp = Time.time;

        string visibleName = phone.GetVisibleName();

        notificationTitle.text = "New Phone Number Discovered!";
        notificationDescription.text = "Check your notebook for " + visibleName;

        notificationButton.gameObject.SetActive(true);

        notificationButton.onClick.RemoveAllListeners();
        notificationButton.onClick.AddListener(() => {
            CameraManager.Instance.GoToNotebook();
            Notebook.Instance.GoToPhonebook();
            notificationButton.gameObject.SetActive(false);
        });
    }

    public void NotifyClue(Clue clue)
    {
        notificationTimestamp = Time.time;

        notificationTitle.text = "New Clue Discovered!";
        notificationDescription.text = "Click here for more";

        notificationButton.gameObject.SetActive(true);

        notificationButton.onClick.RemoveAllListeners();
        notificationButton.onClick.AddListener(() => {
            CameraManager.Instance.GoToNotebook();
            Notebook.Instance.GoToClues();
            Notebook.Instance.ExpandClue(clue);
            notificationButton.gameObject.SetActive(false);
        });
    }
}