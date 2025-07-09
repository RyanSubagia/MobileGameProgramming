using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    [SerializeField] GameObject thePlayer;
    [SerializeField] GameObject playerAnim;
    [SerializeField] AudioSource collisionFX;
    [SerializeField] GameObject mainCam;
    [SerializeField] GameObject fadeOut;

    private bool hasCollided = false;

    void OnTriggerEnter(Collider other)
    {
        // Pastikan hanya dijalankan sekali
        if (hasCollided) return;
        hasCollided = true;

        StartCoroutine(CollisionEnd());
    }

    IEnumerator CollisionEnd()
    {
        collisionFX.Play();
        thePlayer.GetComponent<PlayerMovement>().enabled = false;
        playerAnim.GetComponent<Animator>().Play("Stumble Backwards");
        mainCam.GetComponent<Animator>().Play("CollisionCam");

        LivesManager.instance.LoseLife();

        yield return new WaitForSeconds(2); 
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(1);

        LivesManager.instance.CheckForGameOver();
    }
}