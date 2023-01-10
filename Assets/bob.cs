using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bob : InteractableObject
{
    public WaveSpawner waveSpawner;
    public GameObject bobtext;

    protected override void OnCollided(GameObject collidedObject)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInteract();
        }
        hasInteracted = false;
    }

    protected override void OnInteract()
    {
        if (!hasInteracted)
        {
            hasInteracted = true;
            bobtext.SetActive(false);
            waveSpawner.StartRound();


            Debug.Log("Interacted with " + name);

            this.gameObject.SetActive(false);
        }
    }
}
