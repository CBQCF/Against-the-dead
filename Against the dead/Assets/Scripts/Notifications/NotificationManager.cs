using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using JetBrains.Annotations;
using Mirror.SimpleWeb;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private GameObject notificationMenuPrefab; 
    private GameObject notifMenu;

    public Sprite networkIcon;
    public Sprite errorIcon;
    
    public static NotificationManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }

            instance = FindObjectOfType<NotificationManager>();

            if (instance != null)
            {
                return instance;
            }

            CreateNewInstance();

            return instance;

        }
    }

    private static NotificationManager CreateNewInstance()
    {
        NotificationManager notificationManagerPrefab = Resources.Load<NotificationManager>("NotificationManager");
        instance = Instantiate(notificationManagerPrefab);
        return instance;
    }
    
    private static NotificationManager instance;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        
        notifMenu = Instantiate(notificationMenuPrefab);
        notifMenu = notifMenu.transform.GetChild(0).gameObject;
    }

    public Notification AddNotification()
    {
        var notification = Instantiate(notificationPrefab, notifMenu.transform);
        return notification.GetComponent<Notification>();
    }
    
    public void SuccessNetwork(string msg)
    {
        Notification notif = AddNotification();
        notif.icon.sprite = networkIcon;
        notif.text.text = msg;
        notif.icon.color = new Color32(29, 131, 72, 255);
        notif.background.color = new Color32(40, 180, 99, 255);
    }
    
    public void Error(string msg)
    {
        Notification notif = NotificationManager.Instance.AddNotification();
        notif.text.text = msg;
        notif.icon.sprite = errorIcon;
        notif.icon.color = new Color32(153, 15, 2, 255);
        notif.background.color = new Color32(208, 49, 45, 255);
    }


}
