using System.Collections;
using System.Collections.Generic;
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

    public bool gunPartChange = false;

    private void Start()
    {
        GetComponent<Shooting>().ResetStats();
        UpdateStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (gunPartChange)
        {
            gunPartChange = false;
            GetComponent<Shooting>().ResetStats();
            UpdateStats();
        }
    }

    public void SwapPart(PartItemData newGunPart)
    {
        switch (newGunPart.partType)
        {
            case (PartItemData.GunPartType.Barrel):
                DropPart(equippedBarrel);
                equippedBarrel = newGunPart;
                gunPartChange = true;
                break;
            case (PartItemData.GunPartType.Trigger):
                DropPart(equippedTrigger);
                equippedTrigger = newGunPart;
                gunPartChange = true;
                break;
            case (PartItemData.GunPartType.Magazine):
                DropPart(equippedMagazine);
                equippedMagazine = newGunPart;
                gunPartChange = true;
                break;
            case (PartItemData.GunPartType.Stock):
                DropPart(equippedStock);
                equippedStock = newGunPart;
                gunPartChange = true;
                break;
            case (PartItemData.GunPartType.Sight):
                DropPart(equippedSight);
                equippedSight = newGunPart;
                gunPartChange = true;
                break;
            case (PartItemData.GunPartType.Special):
                equippedSpecials.Add(newGunPart); // Redo later - Should only be 1 able to equipped unless other items are unlocked(?)
                gunPartChange = true;
                break;
        }
    }

    void DropPart(PartItemData newGunPart)
    {
        Instantiate(newGunPart.prefab, GetComponent<Transform>().position, GetComponent<Transform>().rotation);
    }

    void UpdateStats()
    {
        Shooting s = GetComponent<Shooting>();
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
    void UpdateBarrel(Shooting s)
    {
        // Barrel affects bullet speed, # of bullets fired, fire rate, spread, range
        s.ChangeSpeed(equippedBarrel.bulletForce);
        s.ChangeNumberOfBullets(equippedBarrel.numberOfBullets);
        s.ChangeFireRate(equippedBarrel.fireRateMultiplier);
        s.ChangeSpreadAngle(equippedBarrel.accuracyMultiplier);
        s.ChangeRange(equippedBarrel.range);
    }

    void UpdateTrigger(Shooting s)
    {
        // Trigger affects fire rate, and whether the gun is semi auto or not
        s.ChangeFireRate(equippedTrigger.fireRateMultiplier);
        s.isAuto = equippedTrigger.isAuto;
    }

    void UpdateMagazine(Shooting s)
    {
        // Magazine affect max ammo, damage, range
        s.ChangeMaxAmmo(equippedMagazine.maxAmmo);
        s.ChangeDamage(equippedMagazine.damage);
        s.ChangeRange(equippedMagazine.range);
    }

    void UpdateStock(Shooting s)
    {
        // Stock affects accuracy, bullet spread
        s.ChangeSpreadAngle(equippedStock.accuracyMultiplier);
    }

    void UpdateSight(Shooting s)
    {
        // Sight affect accuracy
        s.ChangeSpreadAngle(equippedSight.accuracyMultiplier);
    }

    void UpdateSpecial(Shooting s)
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
}
