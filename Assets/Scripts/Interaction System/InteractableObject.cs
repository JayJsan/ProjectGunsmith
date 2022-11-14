using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : CollidableObject
{
    protected bool hasInteracted = false;
    protected override void OnCollided(GameObject collidedObject)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInteract();
        }
    }

    protected virtual void OnInteract()
    {
        if (!hasInteracted)
        {
            hasInteracted = true;
            Debug.Log("Interacted with " + name);
        }
    }
}
