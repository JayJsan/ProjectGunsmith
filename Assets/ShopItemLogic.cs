using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemLogic : MonoBehaviour
{
    public GameObject[] possibleShopItems;
    public GameObject[] currentShopItems;
    public InventoryManager playerInventoryManager;
    public ShopUILogic shopUI;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ShopUILogic>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        } else
        {
            // this guy broke fr
            shopUI.PlayerIsBroke();
        }
    }
}
