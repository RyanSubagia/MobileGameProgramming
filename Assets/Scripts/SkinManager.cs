using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SkinData
{
    public string skinName;
    public string skinID;
    public GameObject skinPrefab;
}

public class SkinManager : MonoBehaviour
{
    public static SkinManager instance;
    public List<SkinData> allSkins;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UnlockSkin(string skinID)
    {
        PlayerPrefs.SetInt(skinID, 1); 
        PlayerPrefs.Save();
    }

    public bool IsSkinUnlocked(string skinID)
    {

        string defaultSkinID = "com.polyrun.game.skin_default";

        if (skinID == defaultSkinID)
        {
            return true;
        }

        return PlayerPrefs.GetInt(skinID, 0) == 1;
    }

    public void SelectSkin(string skinID)
    {
        PlayerPrefs.SetString("SelectedSkinID", skinID);
        PlayerPrefs.Save();
    }

    public GameObject GetSelectedSkinPrefab()
    {
        string selectedID = PlayerPrefs.GetString("SelectedSkinID", "com.polyrun.game.skin_default");
        foreach (var skin in allSkins)
        {
            if (skin.skinID == selectedID) return skin.skinPrefab;
        }
        return allSkins[0].skinPrefab; 
    }
}