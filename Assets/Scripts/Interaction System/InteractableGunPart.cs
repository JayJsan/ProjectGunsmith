using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableGunPart : InteractableObject
{
    public PartItemData gunPartData;

    private void Awake()
    {
        // Add random gold to gunpart
        gunPartData.cost += Random.Range(0, gunPartData.randomCostRange);
    }

    protected override void Update()
    {
        _collider2D.OverlapCollider(_filter2D, _collidedObjects);
        foreach (var o in _collidedObjects)
        {
            if (o.gameObject.tag == "Player")
            {
                OnCollided(o.gameObject);
            }
        }
    }
    protected override void OnInteract()
    {
        SwapGunPart();
    }

    private void SwapGunPart()
    {
        // Swap new part with old part
        InventoryManager inventoryManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();
        inventoryManager.SwapPart(gunPartData, GetComponent<Transform>());
        // Destroy new part prefab in worldspace
        Destroy(this.gameObject);
    }
}
