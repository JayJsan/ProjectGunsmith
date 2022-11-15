using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Shooting : MonoBehaviour
{
    #region Components
    public Transform firePoint;
    public Transform gunPivot;
    public Transform entityTransform;
    public GameObject bulletPrefab;
    public GameObject AmmoText;
    #endregion

    #region Gun Stat Variables
    // Speed
    private float defaultBulletForce = 0f;
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
    private float lastShootTime = 0f;

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

    // Determines if weapon is semi-auto or automatic
    public bool isAuto = false;
    #endregion

    #region Variables
    public bool enableShoot;
    public bool isReloading = false;
    #endregion

    private void Start()
    {
        enableShoot = true;
        isReloading = false;
        entityTransform = GetComponent<Transform>();
        AmmoText = GameObject.Find("Canvas/AmmoDisplay/AmmoText");
        HandleAmmoText(isReloading);
    }

    // Update is called once per frame
    void Update()
    {
        HandleReload();
        if (enableShoot)
        {
            HandleInputs();
        }
    }

    void HandleInputs()
    {
        if (isAuto)
        {
            if (Input.GetButton("Fire1"))
            {
                Cooldown();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Cooldown();
            }
        }
    }

    void Shoot()
    {
        // For loop is to create number of bullets per shot
        for (int i = 0; i < numberOfBullets; i++)
        {
            #region ACCURACY
            // Reset firePoint rotation
            // firePoint.rotation = entityTransform.rotation;

            // Give random spread of accuracy
            float randomAccuracy = Random.Range(-45 + (45 * spreadAngle), 45 - (45 * spreadAngle));
            firePoint.localRotation = Quaternion.Euler(0, 0, (randomAccuracy));
            #endregion

            // Create bullet
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();

            // Modify Damage
            bullet.GetComponent<Bullet>().damage = damage;

            // Modify Range
            bullet.GetComponent<Bullet>().Range = range;

            // Add force (speed of bullet)
            rbBullet.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
        }
    }

    void Cooldown()
    {
        // Handles Fire Rate
        if (Time.time > lastShootTime + fireRate)
        {
            // Handles ammunition
            if (!(currentAmmo <= 0))
            {
                Shoot();
                currentAmmo--;
                HandleAmmoText(isReloading);
            }
            lastShootTime = Time.time;
        }
    }

    void HandleAmmoText(bool isReloading)
    {
        TextMeshProUGUI TMPComp = AmmoText.GetComponent<TextMeshProUGUI>();
        if (isReloading)
        {
            TMPComp.text = "Reloading";
        }
        else
        {
            TMPComp.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        }

    }

    void HandleReload()
    {
        if ((Input.GetKeyDown(KeyCode.R)) || (currentAmmo <= 0))
        {
            if (currentAmmo == maxAmmo)
            {
                TextMeshProUGUI TMPComp = AmmoText.GetComponent<TextMeshProUGUI>();
                TMPComp.text = "Already full!";
                return;
            }
            if (!isReloading)
            {
                StartCoroutine(Reload());
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        enableShoot = false;
        Debug.Log("Is Reloading...");
        HandleAmmoText(isReloading);
        yield return new WaitForSeconds(reloadTime);
        Debug.Log("Reloaded!");
        currentAmmo = maxAmmo;
        enableShoot = true;
        isReloading = false;
        HandleAmmoText(isReloading);
    }

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
    #endregion
}
