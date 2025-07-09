using UnityEngine;
using Unity.Notifications.Android;
public class NotificationManager : MonoBehaviour
{
    public static NotificationManager instance;

    private const string CHANNEL_ID = "game_channel_01";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = CHANNEL_ID,
            Name = "Game Notifications",
            Importance = Importance.Default,
            Description = "Notifikasi umum dari game.",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    public void SendFullLivesNotification(string title, string text)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.FireTime = System.DateTime.Now.AddSeconds(1);

        AndroidNotificationCenter.SendNotification(notification, CHANNEL_ID);
    }
}