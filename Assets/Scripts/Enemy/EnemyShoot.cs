using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    #region Components
    [Header("Components")]
    public Transform firePoint;
    public Transform gunPivot;
    public Transform entityTransform;
    public Transform playerTransform;
    public GameObject bulletPrefab;
    #endregion

    #region Gun Stat Variables
    // Speed
    [Header("Gun Stats")]
    public float bulletForce = 0f;

    // Accuracy
    // Default is 0.5f:
    // 1f - 100% accurate
    // 0f - Bullet spread spans across a fixed angle.
    [Range(0f, 1f)]
    public float spreadAngle = 0.5f;

    // Damage
    // Self-explanatory
    public int damage = 0;

    // Range
    // Bullet "range" but mechanicall is the bullet lifetime. Will probably change later on.
    public float range = 0f;

    // Fire Rate
    // Default is 1: Being 1 bullet per second. fireRate is measured in time between seconds
    // Bullets per second is well bullets per second.
    [Range(0f, 2f)]
    public float fireRate = 1f;
    private float lastShootTime = 0f;

    // Ammo
    public int maxAmmo = 0;
    public int currentAmmo = 0;

    // Reload Time
    // Default = 1f - Reloads in one second.
    public float reloadTime = 1f;

    // # of bullets shot per fire.
    public int numberOfBullets = 1;

    // Determines if weapon is semi-auto or automatic
    public bool isAuto = false;
    #endregion

    #region Variables
    public bool enableShoot;
    public bool isReloading = false;
    public bool onSight = false;
    #endregion

    private void Start()
    {
        enableShoot = true;
        isReloading = false;
        entityTransform = GetComponent<Transform>();
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

    private void FixedUpdate()
    {
        DetectPlayer();
    }

    void HandleInputs()
    {
        if (onSight)
        {
            Cooldown();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Cooldown();
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
            bullet.GetComponent<EnemyBullet>().damage = damage;

            // Modify Range
            bullet.GetComponent<EnemyBullet>().Range = range;

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
            }
            lastShootTime = Time.time;
        }
    }

    void HandleReload()
    {
        if ((currentAmmo <= 0))
        {
            if (currentAmmo == maxAmmo)
            {
                return;
            }
            if (!isReloading)
            {
                StartCoroutine(Reload());
            }
        }
    }

    void DetectPlayer()
    {
        Vector2 playerDir = playerTransform.position - transform.position;
        int layerMask = ~(LayerMask.GetMask("Damagables"));
        Debug.DrawRay(transform.position, playerDir * 30f);
        var hit = Physics2D.Raycast(transform.position, playerDir, 30f, layerMask);
        Debug.Log("Test");
        if (hit.collider != null)
        {
            Debug.Log("Hit: + " + hit.collider.gameObject.name);
            if (hit.collider.tag == "Player")
            {
                onSight = true;
            }
            else
            {
                onSight = false;
            }
        } else
        {
            Debug.Log("No collider hit?");
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        enableShoot = false;
        Debug.Log("Is Reloading...");
        yield return new WaitForSeconds(reloadTime);
        Debug.Log("Reloaded!");
        currentAmmo = maxAmmo;
        enableShoot = true;
        isReloading = false;
    }

}
