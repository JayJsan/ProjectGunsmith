using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;
public class PlayerShoot : GunStats
{
    #region Components
    [Header("Components")]
    public Transform firePoint;
    public Transform gunPivot;
    public Transform entityTransform;
    public Transform playerBulletHolder;
    public GameObject bulletPrefab;
    public GameObject AmmoText;
    public ObjectPool bulletPool;
    private PlayerHealth playerHealthManager;
    #endregion

    #region Variables
    public bool enableShoot;
    public bool isReloading = false;
    private float lastShootTime = 0f;
    #endregion

    private void Awake()
    {
        playerBulletHolder = GameObject.Find("PlayerBulletHolder").GetComponent<Transform>();
    }

    private void Start()
    {
        enableShoot = true;
        isReloading = false;
        entityTransform = GetComponent<Transform>();
        playerHealthManager = GetComponent<PlayerHealth>();
        AmmoText = GameObject.Find("Canvas/AmmoDisplay/AmmoText");
        HandleAmmoText(isReloading);

    }

    // Update is called once per frame
    void Update()
    {
        if (!playerHealthManager.isPlayerDead)
        {
            HandleReload();
            if (enableShoot)
            {
                HandleInputs();
            }
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
            //GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation, playerBulletHolder); //--- THIS IS HOW WE USED TO INSTIANTIATE
            // IMPLEMENTING OBJECT POOL SYSTEM------------
            GameObject bullet = bulletPool.GetPooledObject();
            if (bullet == null) { return; }
            bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);

            // Modify Bullet Size
            // Reset size first then apply modified size
            bullet.transform.localScale = new Vector3(1, 1, 1);
            bullet.transform.localScale = new Vector3(size, size, 1);

            bullet.SetActive(true);
            // IMPLEMENTING OBJECT POOL SYSTEM -----------

            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            Bullet bulletStats = bullet.GetComponent<Bullet>();
            // Modify Damage
            bulletStats.damage = damage;

            // Modify Range
            bulletStats.Range = range;

            // Modify Piercing
            bulletStats.piercing = piercingAmount;

            // Add force (speed of bullet)
            rbBullet.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);

            // Add camera shake
            CinemachineShake.Instance.ShakeCamera(4f, .05f);
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

}
