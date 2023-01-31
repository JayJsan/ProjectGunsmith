using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int contactDamage = 1;
    public int baseGoldDrop = 1;
    private int lastBulletToHit;

    private int goldDrop;
    public bool randomGoldDrop = false;

    public bool randomDamageOn = false;

    [Range(0, 1000)]
    public int randomDamage = 0;

    public WaveSpawner waveSpawner;
    public SpriteRenderer enemySpriteRenderer;
    #region OnHitFlash
    public static InventoryManager inventoryManager;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;
    public int numberOfFlashes = 1;
    public float flashDuration = 0.1f;
    #endregion

    [Range(0, 1000)]
    public int randomGoldRange = 5;


    public Collider2D _collider;
    private void Awake()
    {
        inventoryManager = GameObject.Find("Player").GetComponent<InventoryManager>();
        waveSpawner = GameObject.Find("GameManager").GetComponent<WaveSpawner>();
        _collider = GetComponent<Collider2D>();
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
    }

    private void Start()
    {
        if (randomGoldDrop)
        {
            goldDrop = baseGoldDrop + Random.Range(0, randomGoldRange);
        }
        else
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
        StartCoroutine(FlashCoroutine());
        if (health <= 0)
        {
            // InventoryManager playerInventory = GameObject.Find("Player").GetComponent<InventoryManager>();
            if (inventoryManager != null)
            {
                inventoryManager.AddGold(goldDrop);
            }
            Die();
        }
    }

    void Die()
    {
        if (waveSpawner.enemiesAlive > 0)
        {
            waveSpawner.SubtractEnemiesAlive(1);
        }
        Destroy(gameObject);
    }

    private IEnumerator FlashCoroutine()
    {
        int currentFlash = 0;
        while (currentFlash < numberOfFlashes)
        {
            enemySpriteRenderer.material.shader = shaderGUItext;
            enemySpriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashDuration);
            enemySpriteRenderer.material.shader = shaderSpritesDefault;
            enemySpriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashDuration);
            currentFlash++;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == ("Bullet"))
        {
            if (lastBulletToHit != hit.gameObject.GetInstanceID())
                TakeDamage(hit.gameObject.GetComponent<Bullet>().damage);
            lastBulletToHit = hit.gameObject.GetInstanceID();
        }
    }
}
