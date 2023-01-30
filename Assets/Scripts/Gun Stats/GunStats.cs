using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStats : MonoBehaviour
{
    #region Gun Stat Variables
    // The reason there are three types of stats (default, current, and no prefix) is due to the way I change the stats in the inventory manager.
    // If I use only one variable to keep track of the stats, I cannot reset it properly?? god i need to do this way better
    //  -- I will refactor this one day but for now it works for now --
    //
    // Speed
    private float defaultBulletForce = 0f;
    [Header("Gun Stats")]
    public float bulletForce = 0f;

    // Accuracy
    // Default is 0.5f:
    // 1f - 100% accurate
    // 0f - Bullet spread spans across a fixed angle.
    private float defaultSpreadAngle = 0.5f;
    [Range(0f, 1f)]
    public float spreadAngle = 0.5f;
    private float defaultAccuracyMultiplier = 0f; // (defaultMultiplier + 1) * spread angle = no change
    public float currentAccuracyMultiplier = 0f;
    // Damage
    // Self-explanatory
    private int defaultDamage = 0;
    private int currentDamage = 0;
    public int damage = 0;

    // Range
    // Bullet "range" but mechanicall is the bullet lifetime. Will probably change later on.
    private float defaultRange = 1f;
    private float currentRange = 0f;
    public float range = 0f;

    // Fire Rate
    // Default is 1: Being 1 bullet per second. fireRate is measured in time between seconds
    // Bullets per second is well bullets per second.
    private float defaultFireRate = 1f;
    private float defaultBulletsPerSecond = 1f;
    [Range(0f, 2f)]
    public float fireRate = 1f;
    public float currentBulletsPerSecond = 1f;

    // Ammo
    private int defaultMaxAmmo = 0;
    public int maxAmmo = 0;
    public int currentAmmo = 0;

    // Reload Time
    // Default = 1f - Reloads in one second.
    private float defaultReloadTime = 1f;
    public float reloadTime = 1f;

    // # of bullets shot per fire.
    private int defaultNumberOfBullets = 1;
    public int numberOfBullets = 0;

    // Piercing - Allows the bullet to damage through an enemy and disappears after x amount of enemy collisions.
    public int piercingAmount = 0;
    private int defaultPiercingAmount = 0;
    private int currentPiercingAmount = 0;

    // Size - Changes the size of the bullet 
    public float size = 1f;
    private float defaultSize = 1f;
    private float currentSize = 1f;

    // Determines if weapon is semi-auto or automatic
    public bool isAuto = false;
    #endregion

    #region Stat Change Methods

    public void ResetStats()
    {
        this.bulletForce = defaultBulletForce;
        this.spreadAngle = defaultSpreadAngle;
        this.currentAccuracyMultiplier = defaultAccuracyMultiplier;
        this.damage = defaultDamage;
        this.currentDamage = defaultDamage;
        this.range = defaultRange;
        this.currentRange = defaultRange;
        this.fireRate = defaultFireRate;
        this.currentBulletsPerSecond = defaultBulletsPerSecond;
        this.maxAmmo = defaultMaxAmmo;
        this.reloadTime = defaultReloadTime;
        this.numberOfBullets = defaultNumberOfBullets;
        this.piercingAmount = defaultPiercingAmount;
        this.size = defaultSize;
        this.isAuto = false;
    }

    public void ChangeSpeed(float newBulletForce)
    {
        this.bulletForce = defaultBulletForce + newBulletForce;
    }

    public void ChangeSpreadAngle(float accuracyMultiplier)
    {
        this.spreadAngle = defaultSpreadAngle * (currentAccuracyMultiplier + accuracyMultiplier + 1);
        currentAccuracyMultiplier += accuracyMultiplier;
    }

    public void ChangeDamage(int newDamage)
    {
        this.damage = currentDamage + newDamage;
        currentDamage += newDamage;
    }

    public void ChangeRange(float newRange)
    {
        this.range = currentRange + newRange;
        currentRange += newRange;
    }

    public void ChangeFireRate(float fireRateMultiplier)
    {
        // Fire rate is currently measured as time between shots instead of bullets per second
        // to make it easier for myself, fire rate multipliers will be applied on bullets per seconds and then changed to time between shots
        this.fireRate = 1 / (currentBulletsPerSecond * (fireRateMultiplier + 1));
        currentBulletsPerSecond += currentBulletsPerSecond * (fireRateMultiplier + 1);
    }

    public void ChangeMaxAmmo(int newMaxAmmo)
    {
        this.maxAmmo = defaultMaxAmmo + newMaxAmmo;
    }

    public void ChangeReloadTime(float reloadTimeMultiplier)
    {
        this.reloadTime = defaultReloadTime * (reloadTimeMultiplier + 1);
    }

    public void ChangeNumberOfBullets(int newNumberOfBullets)
    {
        this.numberOfBullets = newNumberOfBullets;
    }

    public void ChangePiercing(int newPiercing)
    {
        this.piercingAmount = currentPiercingAmount + newPiercing;
        currentPiercingAmount += newPiercing;
    }

    // size will be increased by percentage
    public void ChangeSize(float multiplier)
    {
        this.size = currentSize * multiplier;
        currentSize *= multiplier;
    }
    #endregion
}
