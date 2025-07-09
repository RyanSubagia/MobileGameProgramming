using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    private SkinManager skinManager;
    public Button[] skinButtons;

    void Awake()
    {
        skinManager = SkinManager.instance;
        if (skinManager == null)
        {
            Debug.LogError("SkinManager.instance belum siap atau tidak ditemukan!");
        }
    }

    void OnEnable()
    {
        if (skinManager != null)
        {
            UpdateShopUI();
        }
    }

    public void UpdateShopUI()
    {

        string defaultSkinID = "com.polyrun.game.skin_default";
        string selectedSkinID = PlayerPrefs.GetString("SelectedSkinID", defaultSkinID);

        for (int i = 0; i < skinManager.allSkins.Count; i++)
        {
            SkinData currentSkin = skinManager.allSkins[i];
            Button currentButton = skinButtons[i];
            TextMeshProUGUI buttonText = currentButton.GetComponentInChildren<TextMeshProUGUI>();

            if (skinManager.IsSkinUnlocked(currentSkin.skinID))
            {

                currentButton.onClick.RemoveAllListeners();

                if (selectedSkinID == currentSkin.skinID)
                {
                    buttonText.text = "Used";
                    currentButton.interactable = false; 
                }
                else
                {
                    buttonText.text = "Use";
                    currentButton.interactable = true; 
                    int skinIndex = i;
                    currentButton.onClick.AddListener(() => SelectSkin(skinIndex));
                }
            }
            else
            {
                currentButton.interactable = true;
            }
        }
    }

    void SelectSkin(int skinIndex)
    {
        skinManager.SelectSkin(skinManager.allSkins[skinIndex].skinID);
        UpdateShopUI(); 
    }


    public void OnSkinPurchased()
    {
        UpdateShopUI();
    }
}