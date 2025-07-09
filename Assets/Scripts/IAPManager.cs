using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    public void OnPurchaseComplete(Product product)
    {
        if (product != null)
        {
            SkinManager.instance.UnlockSkin(product.definition.id);
            FindObjectOfType<ShopManager>()?.OnSkinPurchased();
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.LogWarning($"Pembelian gagal untuk {product.definition.id}, alasan: {reason}");
    }
}