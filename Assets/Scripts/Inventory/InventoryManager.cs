using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private List<InventoryItemData> inventoryList = new List<InventoryItemData>();
    #region GUN PART LIST
    public PartItemData equippedBarrel;
    public PartItemData equippedTrigger;
    public PartItemData equippedMagazine;
    public PartItemData equippedStock;
    public PartItemData equippedSight;
    public List<PartItemData> equippedSpecials = new List<PartItemData>();
    #endregion

    #region OTHER ITEMS LIST
    public int goldAmount = 0;
    #endregion

    #region UI
    public TextMeshProUGUI partsUI;
    public TextMeshProUGUI statsUI;
    public TextMeshProUGUI goldUI;
    public PlayerShoot shootManager;
    public GameObject menu;
    #endregion

    private bool gunPartChange = false;
    private bool inventoryOpen = false;

    private void Start()
    {
        if (partsUI == null)
        {
            partsUI = GameObject.Find("Canvas/Menu/PartDisplay/Parts").GetComponent<TextMeshProUGUI>();
        }
        if (statsUI == null)
        {
            statsUI = GameObject.Find("Canvas/Menu/StatDisplay/Stats").GetComponent<TextMeshProUGUI>();
        }
        if (goldUI == null)
        {
            goldUI = GameObject.Find("Canvas/GoldDisplay/GoldText").GetComponent<TextMeshProUGUI>();
        }
        if (menu == null)
        {
            menu = GameObject.Find("Canvas/Menu");
        }
        shootManager = GetComponent<PlayerShoot>();

        shootManager.ResetStats();
        UpdateStats();
        CloseInventory();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventoryOpen)
            {
                CloseInventory();
            }
            else
            {
                UpdateStatsUI();
                OpenInventory();
            }
        }
        if (gunPartChange)
        {
            gunPartChange = false;
            GetComponent<PlayerShoot>().ResetStats();
            UpdateStats();
            UpdateStatsUI();
        }
    }

    #region Gun Part Methods

    public void SwapPart(PartItemData newGunPart, Transform gunPartPosition)
    {
        switch (newGunPart.partType)
        {
            case (PartItemData.GunPartType.Barrel):
                DropPart(equippedBarrel, gunPartPosition);
                equippedBarrel = newGunPart;
                gunPartChange = true;
                break;
            case (PartItemData.GunPartType.Trigger):
                DropPart(equippedTrigger, gunPartPosition);
                equippedTrigger = newGunPart;
                gunPartChange = true;
                break;
            case (PartItemData.GunPartType.Magazine):
                DropPart(equippedMagazine, gunPartPosition);
                equippedMagazine = newGunPart;
                gunPartChange = true;
                break;
            case (PartItemData.GunPartType.Stock):
                DropPart(equippedStock, gunPartPosition);
                equippedStock = newGunPart;
                gunPartChange = true;
                break;
            case (PartItemData.GunPartType.Sight):
                DropPart(equippedSight, gunPartPosition);
                equippedSight = newGunPart;
                gunPartChange = true;
                break;
            case (PartItemData.GunPartType.Special):
                equippedSpecials.Add(newGunPart); // Redo later - Should only be 1 able to equipped unless other items are unlocked(?)
                gunPartChange = true;
                break;
        }
    }

    #region Gold Methods
    public void AddGold(int gold)
    {
        goldAmount += gold;
        UpdateGold();
    }

    public void UpdateGold()
    {
        goldUI.text = "Gold: " + goldAmount;
    }

    // returns true if bro isn't broke
    public bool SubtractGold(int gold)
    {
        if (gold > goldAmount)
        {
            Debug.Log("this guy broke");
            return false;
        }
        UpdateGold();
        goldAmount -= gold;
        return true;
    }
    #endregion

    void DropPart(PartItemData newGunPart, Transform gunPartPosition)
    {
        Instantiate(newGunPart.prefab, gunPartPosition.position, gunPartPosition.rotation);
    }

    void UpdateStats()
    {
        PlayerShoot s = GetComponent<PlayerShoot>();
        // Barrel affects bullet speed, # of bullets fired, fire rate, spread
        UpdateBarrel(s);

        // Trigger affects fire rate, and whether the gun is semi auto or not
        UpdateTrigger(s);

        // Magazine affect max ammo, damage
        UpdateMagazine(s);

        // Stock affects accuracy, bullet spread
        UpdateStock(s);

        // Sight affect accuracy
        UpdateSight(s);

        // Special affects everything depending on what it is.
        UpdateSpecial(s);
    }

    #region Change Stats Methods
    void UpdateBarrel(PlayerShoot s)
    {
        // Barrel affects bullet speed, # of bullets fired, fire rate, spread, range
        s.ChangeSpeed(equippedBarrel.bulletForce);
        s.ChangeNumberOfBullets(equippedBarrel.numberOfBullets);
        s.ChangeFireRate(equippedBarrel.fireRateMultiplier);
        s.ChangeSpreadAngle(equippedBarrel.accuracyMultiplier);
        s.ChangeRange(equippedBarrel.range);
    }

    void UpdateTrigger(PlayerShoot s)
    {
        // Trigger affects fire rate, and whether the gun is semi auto or not
        s.ChangeFireRate(equippedTrigger.fireRateMultiplier);
        s.isAuto = equippedTrigger.isAuto;
    }

    void UpdateMagazine(PlayerShoot s)
    {
        // Magazine affect max ammo, damage, range, reload time
        s.ChangeMaxAmmo(equippedMagazine.maxAmmo);
        s.ChangeDamage(equippedMagazine.damage);
        s.ChangeRange(equippedMagazine.range);
        s.ChangeReloadTime(equippedMagazine.reloadTime);
    }

    void UpdateStock(PlayerShoot s)
    {
        // Stock affects accuracy, bullet spread
        s.ChangeSpreadAngle(equippedStock.accuracyMultiplier);
    }

    void UpdateSight(PlayerShoot s)
    {
        // Sight affect accuracy
        s.ChangeSpreadAngle(equippedSight.accuracyMultiplier);
    }

    void UpdateSpecial(PlayerShoot s)
    {
        // Special affects everything depending on what it is.
        foreach (PartItemData equippedSpecial in equippedSpecials)
        {
            s.ChangeDamage(equippedSpecial.damage);
            s.ChangeFireRate(equippedSpecial.fireRateMultiplier);
            s.ChangeMaxAmmo(equippedSpecial.maxAmmo);
            s.ChangeNumberOfBullets(equippedSpecial.numberOfBullets);
            s.ChangeRange(equippedSpecial.range);
            s.ChangeReloadTime(equippedSpecial.reloadTime);
            s.ChangeSpeed(equippedSpecial.bulletForce);
            s.ChangeSpreadAngle(equippedSpecial.accuracyMultiplier);
        }
    }
    #endregion

    #endregion

    #region UI 

    void UpdateStatsUI()
    {
        partsUI.text = string.Format("Barrel: {0}\r\n" +
            "Trigger: {1}\r\n" +
            "Magazine: {2}\r\n" +
            "Stock: {3}\r\n" +
            "Sight {4}\r\n" +
            "Specials: n/a\r\n",
            equippedBarrel.displayName,
            equippedTrigger.displayName,
            equippedMagazine.displayName,
            equippedStock.displayName,
            equippedSight.displayName);

        string triggerType;

        float convertedSpreadAngle = Mathf.Abs(shootManager.spreadAngle - 1) * 100;
        if (shootManager.isAuto)
        {
            triggerType = "Auto";
        }
        else
        {
            triggerType = "Semi-Auto";
        }
        statsUI.text = string.Format("Bullet Speed: {0}\r\n" +
            "Spread: {1}\r\n" +
            "Damage: {2}\r\n" +
            "Range: {3}\r\n" +
            "Bullets per second: {4}\r\n" +
            "Max Ammo: {5}\r\n" +
            "Bullets per shot:  {6}\r\n" +
            "Trigger Type: {7}",
            shootManager.bulletForce,
            convertedSpreadAngle,
            shootManager.damage,
            shootManager.range,
            shootManager.currentBulletsPerSecond,
            shootManager.maxAmmo,
            shootManager.numberOfBullets,
            triggerType);
    }
    void OpenInventory()
    {
        inventoryOpen = true;
        menu.SetActive(true);
        menu.SetActive(true);
    }

    void CloseInventory()
    {
        inventoryOpen = false;
        menu.SetActive(false);
        menu.SetActive(false);
    }
    #endregion
}
