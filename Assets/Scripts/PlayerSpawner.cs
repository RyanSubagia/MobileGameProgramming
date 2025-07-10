using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Awake()
    {
        if (SkinManager.instance != null)
        {
            GameObject skinToSpawn = SkinManager.instance.GetSelectedSkinPrefab();
            Instantiate(skinToSpawn, transform.position, Quaternion.identity);
            DynamicGI.UpdateEnvironment();

        }
    }
}