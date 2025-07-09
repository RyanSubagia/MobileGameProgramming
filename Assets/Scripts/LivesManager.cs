using UnityEngine;
using System;
using TMPro; 

public class LivesManager : MonoBehaviour
{
    public static LivesManager instance;

    public int maxLives = 5;
    public int currentLives;
    public float rechargeCooldown = 60f; 

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI rechargeTimerText;
    public GameObject outOfLivesPanel;
    public GameObject adsButton;

    private long nextLifeTime;

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
        LoadLives();
        UpdateUI();
    }

    void Update()
    {
        if (currentLives < maxLives)
        {
            if (nextLifeTime > 0)
            {
                long currentTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
                long remainingTime = nextLifeTime - currentTime;

                if (remainingTime > 0)
                {
                    TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTime);
                    rechargeTimerText.text = string.Format("+1 in : {0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
                }
                else
                {
                    AddLife();
                }
            }
        }
        else
        {
            rechargeTimerText.text = "Full";
        }
    }

    void LoadLives()
    {
        currentLives = PlayerPrefs.GetInt("CurrentLives", maxLives);
        string nextLifeTimeString = PlayerPrefs.GetString("NextLifeTime", "0");
        nextLifeTime = long.Parse(nextLifeTimeString);

        // Cek berapa banyak nyawa yang seharusnya sudah ter-recharge saat game tidak aktif
        if (currentLives < maxLives && nextLifeTime > 0)
        {
            long currentTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            long timePassed = currentTime - nextLifeTime;
            if (timePassed >= 0)
            {
                int livesToRecharge = (int)(timePassed / rechargeCooldown) + 1;
                currentLives = Mathf.Min(currentLives + livesToRecharge, maxLives);

                if (currentLives < maxLives)
                {
                    long newNextLifeTime = nextLifeTime + (long)(livesToRecharge * rechargeCooldown);
                    PlayerPrefs.SetString("NextLifeTime", newNextLifeTime.ToString());
                    nextLifeTime = newNextLifeTime;
                }
                else
                {
                    PlayerPrefs.SetString("NextLifeTime", "0");
                    nextLifeTime = 0;
                }
            }
        }
        PlayerPrefs.SetInt("CurrentLives", currentLives);
    }

    void AddLife()
    {
        currentLives++;
        if (currentLives >= maxLives)
        {
            currentLives = maxLives;
            nextLifeTime = 0;
            PlayerPrefs.SetString("NextLifeTime", "0");

            NotificationManager.instance.SendFullLivesNotification("Lives restores", "All of your life was full lets play again!");
        }
        else
        {
            long currentTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            nextLifeTime = currentTime + (long)rechargeCooldown;
            PlayerPrefs.SetString("NextLifeTime", nextLifeTime.ToString());
        }
        PlayerPrefs.SetInt("CurrentLives", currentLives);
        UpdateUI();
    }

    public void LoseLife()
    {
        if (currentLives > 0)
        {
            bool wasFull = (currentLives == maxLives);
            currentLives--;
            PlayerPrefs.SetInt("CurrentLives", currentLives);

            if (wasFull)
            {
                long currentTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
                nextLifeTime = currentTime + (long)rechargeCooldown;
                PlayerPrefs.SetString("NextLifeTime", nextLifeTime.ToString());
            }

            UpdateUI();
        }
    }

    public void RefillLivesFromAd()
    {
        currentLives = maxLives;
        nextLifeTime = 0;
        PlayerPrefs.SetInt("CurrentLives", currentLives);
        PlayerPrefs.SetString("NextLifeTime", "0");
        outOfLivesPanel.SetActive(false); 
        UpdateUI();

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void CheckForGameOver()
    {
        if (currentLives <= 0)
        {
            outOfLivesPanel.SetActive(true);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    void UpdateUI()
    {
        livesText.text = "Lives : " + currentLives;
    }
}