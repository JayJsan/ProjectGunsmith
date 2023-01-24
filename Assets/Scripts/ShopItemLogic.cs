using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemLogic : MonoBehaviour
{
    public GameObject[] AllPossibleShopItems;
    public GameObject[] currentPossibleShopItems;
    public GameObject[] shopSlots;
    public InventoryManager playerInventoryManager;
    public ShopUILogic shopUI;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ShopUILogic>();
    }

    private void OnEnable()
    {
        ShuffleShop();
    }

    // This method is used by the ShopTriggerLogic script
    public void PlayerBuy(GameObject shopItem)
    {
        // get part item data
        PartItemData shopItemData = shopItem.GetComponent<InteractableGunPart>().gunPartData;

        // find player position
        Transform playerPosition = GameObject.Find("Player").GetComponent<Transform>();

        // subtract gold
        if (playerInventoryManager.SubtractGold(shopItemData.cost))
        {
            // swap gun part
            playerInventoryManager.SwapPart(shopItemData, playerPosition);

            // Remove shop item from shop
            Destroy(shopItem);
        }
        else
        {
            // this guy broke fr
            shopUI.PlayerIsBroke();
        }
    }

    public void ShuffleShop()
    {
        // need to implement a way to make sure the same item isnt displayed 
        //
        // test pseudo code
        // create list of items
        // pick random item
        // remove from list
        //

        foreach (GameObject slot in shopSlots)
        {
            slot.GetComponent<ShopTriggerLogic>().ChangeShopItem(currentPossibleShopItems[Random.Range(0, currentPossibleShopItems.Length - 1)]);
        }
    }
}
