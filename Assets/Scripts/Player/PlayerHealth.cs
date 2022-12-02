using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : CollidableObject
{
    private int defaultPlayerHealth = 6;

    public int currentPlayerHealth = 6;
    public int maxPlayerHealth = 6;
    int damageDealt = 0;

    public bool isPlayerDead = false;
    private bool hasDied = false;
    private GameObject loseText;

    [Header("Invincibiltiy Frames Stuff")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public SpriteRenderer playerSprite;
    // Heart Stuff
    [Header("Heart Stuff")]
    public Image[] hearts = new Image[3];
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public Sprite damagedFullHeart;
    public Sprite damagedHalfHeart;
    public Sprite halfHeartHalfDamaged;

    private int fullHearts;
    private bool hasHalfHeart;
    private int damagedFullHearts;
    private bool hasDamagedHalfHeart;
    private int lastFullHearts;

    public bool hit = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        lastFullHearts = Mathf.FloorToInt(currentPlayerHealth / 2);
        fullHearts = Mathf.FloorToInt(currentPlayerHealth / 2);
        _collider2D = GetComponent<Collider2D>();
        loseText = GameObject.Find("Canvas/LoseScreen/LoseText");
        hearts[0] = GameObject.Find("Canvas/HealthDisplay/Heart1").GetComponent<Image>();
        hearts[1] = GameObject.Find("Canvas/HealthDisplay/Heart2").GetComponent<Image>();
        hearts[2] = GameObject.Find("Canvas/HealthDisplay/Heart3").GetComponent<Image>();

        loseText.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!isPlayerDead)
        {
            _collider2D.OverlapCollider(_filter2D, _collidedObjects);
            foreach (var o in _collidedObjects)
            {
                if (!hit)
                {
                    hit = true;
                    OnCollided(o.gameObject);
                }
            }
        }
    }

    private IEnumerator FlashCoroutine()
    {
        int currentFlash = 0;
        _collider2D.enabled = false;
        UpdateDamageHeartSprite();
        while (currentFlash < numberOfFlashes)
        {
            playerSprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            playerSprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            currentFlash++;
        }
        UpdateHeartSprite();
        CheckIfPlayerHasDied();
        _collider2D.enabled = true;
        hit = false;
    }

    protected override void OnCollided(GameObject collidedObject)
    {

        if (collidedObject.GetComponent<EnemyBullet>() != null)
        {
            damageDealt = collidedObject.GetComponent<EnemyBullet>().damage;
        } else {
            damageDealt = collidedObject.GetComponent<Enemy>().contactDamage;
        }
        currentPlayerHealth -= damageDealt;
        UpdateHeartSprite();
        StartCoroutine(FlashCoroutine());
        Debug.Log("Player hit with " + collidedObject.name);
    }

    public void ResetPlayerHealth()
    {
        this.currentPlayerHealth = defaultPlayerHealth;
    } 

    public void HealPlayerHealtlh()
    {
        this.currentPlayerHealth = maxPlayerHealth;
    }

    public void CheckIfPlayerHasDied()
    {
        if (currentPlayerHealth <= 0)
        {
            isPlayerDead = true;
            if (!hasDied)
            {
                hasDied = true;
                loseText.SetActive(true);
                playerSprite.color = flashColor;
                transform.Rotate(new Vector3(0, 0, 90));
            }
        }
    }

    void UpdateHeartSprite()
    {
        lastFullHearts = fullHearts;
        fullHearts = Mathf.FloorToInt(currentPlayerHealth / 2);
        hasHalfHeart = !(currentPlayerHealth % 2 == 0);
        damagedFullHearts = Mathf.FloorToInt(damageDealt / 2);
        hasDamagedHalfHeart = !(damageDealt % 2 == 0);

        Debug.Log("Full hearts: " + fullHearts);
        Debug.Log("halfHeart?: " + hasHalfHeart);

        // Calculating what hearts need to be displayed
        // Initialise all hearts as empty first
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = emptyHeart;
        }

        for (int i = 0; i < fullHearts; i++)
        {
            hearts[i].sprite = fullHeart;
        }

        if (hasHalfHeart)
        {
            hearts[fullHearts].sprite = halfHeart;
        }
    }

    void UpdateDamageHeartSprite()
    {
        // Calculating how many white heart damaged sprites need to be displayed
        /*
        for (int i = Mathf.FloorToInt(currentPlayerHealth / 2); i > (Mathf.FloorToInt((currentPlayerHealth - damageDealt) / 2)); i--)
        {
            hearts[i].sprite = damagedFullHeart;
            Debug.Log("FORLOOPENTERED");
        }
        */
        /*
        for (int i = fullHearts; i <= (fullHearts + damagedFullHearts); i++)
        {
            hearts[i].sprite = damagedFullHeart;
        }

        */
        /*
        Debug.Log("test: " + (lastFullHearts - damagedFullHearts - 1));
        for (int i = lastFullHearts - 1 ; i >= (lastFullHearts - damagedFullHearts - 1); i--)
        {
            Debug.Log(i);
            hearts[i].sprite = damagedFullHeart;
        }
        */

        if (hasDamagedHalfHeart)
        {
            if (hasHalfHeart)
            {
                hearts[fullHearts].sprite = halfHeartHalfDamaged;
            }
            else
            {
                hearts[fullHearts].sprite = damagedHalfHeart;
            }
        }
    }
}
