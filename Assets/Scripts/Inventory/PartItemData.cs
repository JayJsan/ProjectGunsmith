using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gun Part Item Data")]
public class PartItemData : ScriptableObject
{
    public enum GunPartType
    {
        Barrel, Trigger, Magazine, Stock, Sight, Special, Empty
    }
    public string id;
    public string displayName;
    public Sprite partSprite;
    public GameObject prefab;
    public GunPartType partType;

    #region Gun Stat Variables
    // Speed
    public float bulletForce = 0f;

    // Accuracy
    // Default is 0f:
    // 1f - 100% accurate
    // 0f - Bullet spread spans across a fixed angle.
    // Player Spread Angle Stat is defaulted to 50% accuracy or 0.5f
    // Spreadangle Stat holder stat will apply a percentage to the default player stat.
    [Range(-1f, 1f)]
    public float accuracyMultiplier = 0f;

    // Damage
    // Self-explanatory
    public int damage = 0;

    // Range
    // Bullet "range" but mechanicall is the bullet lifetime. Will probably change later on.
    public float range = 0f;

    // Fire Rate
    // Player Fire Rate Stat is a float fixed at 1 bullet per second or a 1.0 float.
    // StatHolder stats will apply a percentage to player gun stat. i.e. 0.10 being 10% increase of default fire rate.
    [Range(0f, 10f)]
    public float fireRateMultiplier = 0f;

    // Ammo
    public int maxAmmo = 0;

    // Reload Time
    public float reloadTime = 0f;

    // # of bullets shot per fire.
    public int numberOfBullets = 0;

    // Determines if weapon is semi-auto or automatic
    public bool isAuto = false;

    public int cost = 0;
    [Range(0, 100)]
    public int randomCostRange = 0;

    public int piercingAmount = 0;

    public float sizeMultiplier = 0f;
    #endregion
}
