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
        CheckShopItem();
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
                hasBought = true;
                _collider2D.enabled = false;
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
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        shopItem = Instantiate(newShopItem, transform.position, Quaternion.identity, transform);

        if (shopItem.GetComponent<Collider2D>() != null)
        {
            // disables actual gun part collider so you can't grab it off table for free
            shopItem.GetComponent<Collider2D>().enabled = false;
        }


        hasBought = false;
        _collider2D.enabled = true;
    }

    private void CheckShopItem()
    {
        if (this.gameObject.transform.GetChild(0).gameObject != null)
        {
            shopItem = this.gameObject.transform.GetChild(0).gameObject;
        }
    }
}
