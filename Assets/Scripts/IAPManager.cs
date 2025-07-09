using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    public void OnPurchaseComplete(Product product)
    {
        if (product != null)
        {
            string id = product.definition.id;
            if (id == "com.polyrun.game.coin_multiplier")
            {

                ShakeMinigame minigameInstance = FindObjectOfType<ShakeMinigame>(true); 

                if (minigameInstance != null)
                {
                    Debug.Log("ShakeMinigame FOUND ");
                    minigameInstance.StartMinigame();
                }
                else
                {
                    Debug.LogError("FATAL ERROR:NOT FOUND MINIGAME.");
                }

                MultiplierManager.instance.PrepareMultiplier();
            }
            else 
            {
                SkinManager.instance.UnlockSkin(id);
                FindObjectOfType<ShopManager>()?.OnSkinPurchased();
            }
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.LogWarning($"Purchase Failed {product.definition.id}, reason: {reason}");
    }
}