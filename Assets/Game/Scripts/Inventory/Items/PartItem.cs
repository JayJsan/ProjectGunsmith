using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using System;
using MoreMountains.InventoryEngine;

namespace MoreMountains.TopDownEngine
{
    [CreateAssetMenu(fileName = "InventoryWeapon", menuName = "ProjectRNG/PartItem", order = 0)]
    [Serializable]
    /// <summary>
    /// Gun Part item in ProjectRNG
    /// </summary>
    public class PartItem : InventoryItem
    {
		StatManager statManager;
		Weapon.TriggerModes triggerMode;
		// Projectile to use
		public GameObject projectile;

		[Header("Gun Part Stats Info")]
		#region GUN STAT VARIABLES
		// Bullet ---------------
		public float maxDamage = 0f;
		public float minDamage = 0f;
		public Vector3 knockbackOnTarget = new Vector3(0, 0, 0);
		public float bulletSpeed = 0f;
		public float bulletAcceleration = 0f;
		public float bulletLifeTime = 0f;
		// Gun ---------------
		public int projectilesPerShot;
		public Vector3 bulletSpread = Vector3.zero;
		public Weapon.TriggerModes currentTriggerMode = Weapon.TriggerModes.Unchanged;

		[Range(0f, 2f)]
		public float fireRate = 0f;
		public int magazineSize = 0;
		public float recoilForce = 0f;
		#endregion

		/// <summary>
		/// When we grab the weapon, we equip it
		/// </summary>
		public override bool Equip(string playerID)
		{
			SendGunStats(true);
			return true;
		}
		public override bool UnEquip(string playerID)
		{
			SendGunStats(false);
			// if this is a currently equipped weapon, we unequip it
			if (this.TargetEquipmentInventory(playerID) == null)
			{
				return false;
			}
			if (this.TargetEquipmentInventory(playerID).InventoryContains(this.ItemID).Count > 0)
			{

			}
			

			return true;
		}

		protected void SendGunStats(bool equip)
        {
			statManager = GameObject.FindWithTag("Player").GetComponent<StatManager>();
			if (statManager != null)
            {
				// If item is being equipped, then modify stats.
				// If item is being unequipped, then remove stats modified.
				if (equip)
                {
					statManager.AdjustStats(maxDamage, minDamage, knockbackOnTarget, bulletSpeed, bulletAcceleration,
					bulletLifeTime, projectile, triggerMode, projectilesPerShot, bulletSpread, fireRate, magazineSize, recoilForce);
				} else
                {
					// CHANGE TRIGGERMODE AND PROJECTILE TO UNCHANGED(?) or NoProjectile
					statManager.AdjustStats(-maxDamage, -minDamage, -knockbackOnTarget, -bulletSpeed, -bulletAcceleration,
					-bulletLifeTime, projectile, triggerMode, projectilesPerShot, -bulletSpread, -fireRate, -magazineSize, -recoilForce);
				}
				
            }
        }

	}
}
