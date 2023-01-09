using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int contactDamage = 1;
    public int baseGoldDrop = 1;
    private int goldDrop;
    public bool randomGoldDrop = false;

    public bool randomDamageOn = false;

    [Range(0, 1000)]
    public int randomDamage = 0;

    public static WaveSpawner waveSpawner;
    public static InventoryManager inventoryManager;

    [Range(0,1000)]
    public int randomGoldRange = 5;

    private void Awake()
    {
        inventoryManager = GameObject.Find("Player").GetComponent<InventoryManager>();
        waveSpawner = GameObject.Find("GameManager").GetComponent<WaveSpawner>();
    }

    private void Start()
    {
        if (randomGoldDrop)
        {
            goldDrop = baseGoldDrop + Random.Range(0, randomGoldRange);
        } else
        {
            goldDrop = baseGoldDrop;
        }

        if (randomDamageOn)
        {
            GetComponent<EnemyShoot>().damage += Random.Range(0, randomDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            // InventoryManager playerInventory = GameObject.Find("Player").GetComponent<InventoryManager>();
            if (inventoryManager != null) {
                inventoryManager.AddGold(goldDrop);
            }
            Die();
        }
    }

    void Die()
    {
        if (waveSpawner.enemiesAlive > 0)
        {
            waveSpawner.enemiesAlive -= 1;
        }
        Destroy(gameObject);
    }
}
