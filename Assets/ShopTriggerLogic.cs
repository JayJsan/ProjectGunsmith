using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTriggerLogic : InteractableObject
{
    // REVAMP LATER
    // WRITE OUT DRAFT CODE LATER

    public ShopUILogic shopUILogic;
    public ShopItemLogic shopItemLogic;
    public GameObject shopItem;
    private bool hasBought = false;

    protected override void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        if (shopUILogic == null)
        {
            shopUILogic = GameObject.Find("Shop").GetComponent<ShopUILogic>();
        }

        if (this.gameObject.transform.GetChild(0).gameObject != null)
        {
            shopItem = this.gameObject.transform.GetChild(0).gameObject;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        _collider2D.OverlapCollider(_filter2D, _collidedObjects);
        foreach (var o in _collidedObjects)
        {
            OnCollided(o.gameObject);
        }
    }

    protected override void OnCollided(GameObject collidedObject)
    {
        if (shopItem == null)
        {
            return;
        }

        // On Hover
        // Update UI
        shopUILogic.PlayerIsHovering();

        // gets the shop item game object child of the current parent (shopItem1/"item
        shopUILogic.ChangeUIHoverTarget(shopItem);

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInteract();
        }
    }

    protected override void OnInteract()
    {
        if (!hasInteracted)
        {
            hasInteracted = true;

            if (!hasBought)
            {            
                // pass through shopitem
                shopItemLogic.PlayerBuy(shopItem);
            }



            Debug.Log("Interacted with " + name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            shopUILogic.PlayerIsntHovering();
        }
    }

    public void ChangeShopItem(GameObject newShopItem)
    {
        shopItem = newShopItem;
        hasBought = false;
    }
}
