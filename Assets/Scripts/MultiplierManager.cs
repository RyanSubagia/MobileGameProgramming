using UnityEngine;

public class MultiplierManager : MonoBehaviour
{
    public static MultiplierManager instance;

    public int currentMultiplier { get; private set; } = 1;
    private bool isMultiplierPending = false; 

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void PrepareMultiplier()
    {
        isMultiplierPending = true;
        Debug.Log("Coin Multiplier Purchased.");
    }

    public void SetMultiplier(int multiplier)
    {
        currentMultiplier = multiplier;
        Debug.Log("Multiplier set to : x" + currentMultiplier);
    }

    public void StartOfRun()
    {
        if (isMultiplierPending)
        {
            isMultiplierPending = false;
        }
        else
        {
            currentMultiplier = 1;
        }
        Debug.Log("Run start with multiplier: x" + currentMultiplier);
    }
}