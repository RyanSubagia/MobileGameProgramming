using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System.Collections;

public class ShakeMinigame : MonoBehaviour
{
    public Slider powerBar;
    public TextMeshProUGUI instructionText;
    public float gameDuration = 10f; 
    public float shakeThreshold = 1.5f; 

    private float timer;
    private float shakePower;
    private bool minigameActive = false;

    public void StartMinigame()
    {
        gameObject.SetActive(true);
        minigameActive = true;
        timer = gameDuration;
        shakePower = 0;
        powerBar.value = 0;
    }

    void Update()
    {
        if (!minigameActive) return;

        timer -= Time.deltaTime;

        if (Input.acceleration.sqrMagnitude >= shakeThreshold * shakeThreshold)
        {
            shakePower += Time.deltaTime * 10; 
        }

        // Update UI
        powerBar.value = shakePower / (gameDuration * 5); 
        instructionText.text = "SHAKE UR PHONE TO GET UP TO 5X BONUS \nTIME REMAINING : " + Mathf.Ceil(timer).ToString("F0");

        if (timer <= 0)
        {
            EndMinigame();
        }
    }

    void EndMinigame()
    {
        minigameActive = false;

        int finalMultiplier = 2 + Mathf.RoundToInt(powerBar.value * 8);
        finalMultiplier = Mathf.Clamp(finalMultiplier, 2, 10);

        MultiplierManager.instance.SetMultiplier(finalMultiplier);
        instructionText.text = "YOU GOT\n x" + finalMultiplier + "!";

        StartCoroutine(ClosePanelAfterDelay(3.0f));
    }
    private IEnumerator ClosePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);
    }
}