using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;


public class StatManager : MonoBehaviour
{
    public enum TypeOfWeapon { Projectile, Hitscan, Melee }

    #region COMPONENTS
    public ProjectileWeapon projWeapComp;
    public HitscanWeapon hitWeapComp;
    public MeleeWeapon meleeWeapComp;
    public MoreMountains.Tools.MMSimpleObjectPooler simpleObjectPoolerComp;
    #endregion

    #region REFERENCES
    public GameObject playerWeapon;
    public GameObject projectile;

    // BulletStatsHandler
    private GameObject projectileSceneCopy;
    private bool hasInstantiatedSceneCopy = false;
    #endregion

    #region BASE STAT VARIABLES
    public float baseMaxDamage = 0;
    public float baseMinDamage = 0;
    public Vector3 baseKnockbackOnTarget = Vector3.zero;
    public float baseBulletSpeed = 0;
    public float baseBulletAcceleration = 0;
    public float baseBulletLifeTime = 0;
    // Gun ---------------
    public int baseProjectilesPerShot = 1;
    public Vector3 baseBulletSpread = Vector3.zero;
    public float baseTimeBetweenShot = 1f;
    public int baseMagazineSize = 0;
    public float baseRecoilForce = 0;
    #endregion

    #region GUN STAT VARIABLES
    // Bullet ---------------
    public float currentMaxDamage = 10;
    public float currentMinDamage = 10;
    public Vector3 currentKnockbackOnTarget = new Vector3(10, 10, 0);
    public float currentBulletSpeed = 50f;
    public float currentBulletAcceleration = 0f;
    public float currentBulletLifeTime = 2.5f;
    // Gun ---------------
    TypeOfWeapon typeOfWeapon;
    public Weapon.TriggerModes currentTriggerMode;
    public GameObject currentProjectile;
    public int currentProjectilesPerShot = 1;
    public Vector3 currentBulletSpread = Vector3.zero;

    public float currentTimeBetweenShot = 1f;
    public float totalFireRateIncrease = 0f;
    public int currentMagazineSize = 10;
    public float currentRecoilForce = 0f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Find player's weapon in scene
        playerWeapon = GameObject.FindWithTag("PlayerProjWeapon");
        simpleObjectPoolerComp = playerWeapon.GetComponent<MoreMountains.Tools.MMSimpleObjectPooler>();
        projWeapComp = playerWeapon.GetComponent<ProjectileWeapon>();
        currentProjectile = simpleObjectPoolerComp.GameObjectToPool;;
        CheckCurrentStats();
        SetBaseStats();
        // Check if anything is equipped in inventory and adjust stats accordingly

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AdjustStats(float maxDamage, float minDamage, Vector3 knockbackOnTarget, float bulletSpeed,
        float bulletAcceleration, float bulletLifeTime, GameObject typeOfBullet, Weapon.TriggerModes triggerMode,
        int projectilesPerShot, Vector3 bulletSpread, float fireRate, int magazineSize, float recoilForce)
    {
        // Bullet ---------------
        currentMaxDamage += maxDamage;
        currentMinDamage += minDamage;
        currentKnockbackOnTarget += knockbackOnTarget;
        currentBulletSpeed += bulletSpeed;
        currentBulletAcceleration += bulletAcceleration;
        currentBulletLifeTime += bulletLifeTime;
        // Gun ---------------
        if (typeOfBullet != null)
        {
            // simpleObjectPoolerComp.GameObjectToPool = typeOfBullet;
        }
        // MAKE ANOTHER CLASS TO EXTEND WEAPON TO CHANGE ENUM
        if (triggerMode != Weapon.TriggerModes.Unchanged)
        {
            currentTriggerMode = triggerMode;
        }
        currentProjectilesPerShot += projectilesPerShot;
        currentBulletSpread += bulletSpread;
        totalFireRateIncrease += fireRate;
        currentTimeBetweenShot = 1 / ((1 / baseTimeBetweenShot) * (totalFireRateIncrease + 1));

        currentMagazineSize += magazineSize;
        currentRecoilForce += recoilForce;

        SendStats();
    }
        
    void SendStats()
    {
        switch (typeOfWeapon)
        {
            case TypeOfWeapon.Projectile:
                SendStatsHandler(projWeapComp);
                BulletStatsHandler();
                break;
            case TypeOfWeapon.Hitscan:
                SendStatsHandler(hitWeapComp);
                break;
            case TypeOfWeapon.Melee:
                SendStatsHandler(meleeWeapComp);
                break;
        }
        CheckCurrentStats();
    }

    void SendStatsHandler(ProjectileWeapon projWeapComp)
    {
        projWeapComp.TriggerMode = currentTriggerMode;
        projWeapComp.ProjectilesPerShot = currentProjectilesPerShot;
        projWeapComp.Spread = currentBulletSpread;
        projWeapComp.TimeBetweenUses = currentTimeBetweenShot;
        projWeapComp.MagazineSize = currentMagazineSize;
        projWeapComp.RecoilForce = currentRecoilForce;
    }

    void SendStatsHandler(HitscanWeapon hitWeapComp)

    {

    }

    void SendStatsHandler(MeleeWeapon meleeWeapComp)
    {

    }

    void BulletStatsHandler()
    {
        /*
         Check what prefab the objectpooler component is referencing.
        Take the same reference and instantiate it - making sure it doesn’t get destroyed.
        Freeze the new projectile
        Take the new projectile that's been instantiated and modify its stats.
        Make it inactive?
        Change the MMSimpleObjectPooler reference to the new copy.
        The new copy must stay in the scene at all times.
        */
        
        

        if (!hasInstantiatedSceneCopy)
        {
            projectileSceneCopy = Instantiate(simpleObjectPoolerComp.GameObjectToPool, new Vector3(0, 0, -1), Quaternion.identity);
            projectileSceneCopy.name = "ProjectileSceneCopy";
            hasInstantiatedSceneCopy = true;
        }


        projectileSceneCopy.SetActive(false);
        Projectile projectileComp = projectileSceneCopy.GetComponent<Projectile>();
        DamageOnTouch dmgOTComp = projectileSceneCopy.GetComponent<DamageOnTouch>();
        dmgOTComp.MaxDamageCaused = currentMaxDamage;
        dmgOTComp.MinDamageCaused = currentMinDamage;
        dmgOTComp.DamageCausedKnockbackForce = currentKnockbackOnTarget;
        projectileComp.Speed = currentBulletSpeed;
        projectileComp.Acceleration = currentBulletAcceleration;
        projectileComp.LifeTime = currentBulletLifeTime;

        simpleObjectPoolerComp.GameObjectToPool = projectileSceneCopy;
        currentProjectile = simpleObjectPoolerComp.GameObjectToPool;

        // Remove old projectiles from bullet pool
        simpleObjectPoolerComp.FillObjectPool();
    }

    void CheckCurrentStats()
    {
        Projectile projectileComp = simpleObjectPoolerComp.GameObjectToPool.GetComponent<Projectile>();
        DamageOnTouch dmgOTComp = simpleObjectPoolerComp.GameObjectToPool.GetComponent<DamageOnTouch>();

        // Bullet ---------------
        currentMaxDamage = dmgOTComp.MaxDamageCaused;
        currentMinDamage = dmgOTComp.MinDamageCaused;
        currentKnockbackOnTarget = dmgOTComp.DamageCausedKnockbackForce;
        currentBulletSpeed = projectileComp.Speed;
        currentBulletAcceleration = projectileComp.Acceleration;
        currentBulletLifeTime = projectileComp.LifeTime;
        currentProjectile = simpleObjectPoolerComp.GameObjectToPool;
        // Gun ---------------
        // MAKE ANOTHER CLASS TO EXTEND WEAPON TO CHANGE ENUM
        currentTriggerMode = projWeapComp.TriggerMode;
        currentProjectilesPerShot = projWeapComp.ProjectilesPerShot;
        currentBulletSpread = projWeapComp.Spread;

        currentTimeBetweenShot = projWeapComp.TimeBetweenUses;
        currentMagazineSize = projWeapComp.MagazineSize;
        currentRecoilForce = projWeapComp.RecoilForce;
    }

    void SetBaseStats()
    {
        // Only use this method on awake as it depends on the current stats on initialization
        baseMaxDamage = currentMaxDamage;
        baseMinDamage = currentMinDamage;
        baseKnockbackOnTarget = currentKnockbackOnTarget;
        baseBulletSpeed = currentBulletSpeed;
        baseBulletAcceleration = currentBulletAcceleration;
        baseBulletLifeTime = currentBulletLifeTime;
        
        baseProjectilesPerShot = currentProjectilesPerShot;
        baseBulletSpread = currentBulletSpread;
        baseTimeBetweenShot = currentTimeBetweenShot;
        baseMagazineSize = currentMagazineSize;
        baseRecoilForce = currentRecoilForce;
    }
}
