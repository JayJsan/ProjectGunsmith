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
    public float bulletForce = 20f;

    // Accuracy
    [Range(0f, 1f)]
    public float spreadAngle = 0.75f;

    // Damage
    public int damage = 25;

    // Range
    public float range = 10f;

    // Fire Rate
    [Range(0f, 2f)]
    public float fireRate = 1f;
    private float lastShootTime = 0f;

    // Ammo
    public int maxAmmo = 8;
    public int currentAmmo = 8; 

    // Reload Time
    public float reloadTime = 1f;

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
        } else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Cooldown();
            }
        }
    }

    void Shoot()
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
        } else
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
}
