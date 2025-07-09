using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    [SerializeField] AudioSource collisionFX;
    [SerializeField] GameObject fadeOut;

    private bool hasCollided = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hasCollided) return;
            hasCollided = true;

            GameObject playerObject = other.gameObject;

            StartCoroutine(CollisionEnd(playerObject));
        }
    }

    IEnumerator CollisionEnd(GameObject player)
    {
        collisionFX.Play();
        player.GetComponent<PlayerMovement>().enabled = false;


        Animator[] allAnimators = player.GetComponentsInChildren<Animator>();
        Animator characterAnimator = null; 

        foreach (Animator anim in allAnimators)
        {
            if (anim.gameObject.CompareTag("MainCamera") == false)
            {
                characterAnimator = anim;
                break;
            }
        }

        if (characterAnimator != null)
        {
            characterAnimator.Play("Stumble Backwards");
        }
        else
        {
            Debug.LogWarning("Tidak dapat menemukan Animator milik model karakter.");
        }

        Camera.main.GetComponent<Animator>()?.Play("CollisionCam");


        // Sisa kode lainnya tetap sama...
        if (LivesManager.instance != null)
        {
            LivesManager.instance.LoseLife();
        }

        yield return new WaitForSeconds(2);

        if (fadeOut != null)
        {
            fadeOut.SetActive(true);
        }

        yield return new WaitForSeconds(1);

        if (LivesManager.instance != null)
        {
            LivesManager.instance.CheckForGameOver();
        }
    }
}