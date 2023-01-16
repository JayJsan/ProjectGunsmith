using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : CollidableObject
{
    private int defaultPlayerHealth = 100;

    public int currentPlayerHealth = 100;
    public int maxPlayerHealth = 100;
    private int previousHealth = 100;
    int damageDealt = 0;

    public bool isPlayerDead = false;
    private bool hasDied = false;
    private GameObject loseText;

    [Header("Invincibiltiy Frames Stuff")]
    public Color flashColor;
    public Color playerColor;
    public Color heartColor;
    public float flashDuration;
    public int numberOfFlashes;
    public SpriteRenderer playerSprite;
    [Header("Health Bar")]
    public Slider healthSlider;
    public Slider healthWhiteSlider;
    public GameObject healthFillBar;
    public int lastBulletHitID;
    public float damageBarCountdown = 1.5f;
    private float countdown = 1.5f;
    public bool hit = false;
    private bool hasRun = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        _collidedObjects = new List<Collider2D>();
        // lastFullHearts = Mathf.FloorToInt(currentPlayerHealth / 2);
        // fullHearts = Mathf.FloorToInt(currentPlayerHealth / 2);
        _collider2D = GetComponent<Collider2D>();
        loseText = GameObject.Find("Canvas/LoseScreen/LoseText");
        // hearts[0] = GameObject.Find("Canvas/HealthDisplay/Heart1").GetComponent<Image>();
        // hearts[1] = GameObject.Find("Canvas/HealthDisplay/Heart2").GetComponent<Image>();
        // hearts[2] = GameObject.Find("Canvas/HealthDisplay/Heart3").GetComponent<Image>();
        //healthSlider = GameObject.Find("Canvas/HealthDisplay").GetComponent<Slider>();
        loseText.SetActive(false);

        healthSlider.maxValue = maxPlayerHealth;
        healthWhiteSlider.value = maxPlayerHealth;
        healthWhiteSlider.maxValue = maxPlayerHealth;
        UpdateHealthBarUI();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!isPlayerDead)
        {
            if (countdown > 0)
            {
                countdown -= Time.deltaTime;
            }
            else
            {
                if (!hasRun)
                {
                    UpdateDamageBarUI(previousHealth);
                    previousHealth = currentPlayerHealth;
                    hasRun = true;
                }
            }

            _collider2D.OverlapCollider(_filter2D, _collidedObjects);
            foreach (var o in _collidedObjects)
            {
                if (!hit)
                {
                    //hit = true;
                    countdown = damageBarCountdown;
                    hasRun = false;
                    OnCollided(o.gameObject);
                }
            }
        }
    }

    private IEnumerator FlashCoroutine()
    {
        int currentFlash = 0;
        //_collider2D.enabled = false;
        //UpdateHealthBarUIDamaged(previousHealth);
        while (currentFlash < numberOfFlashes)
        {
            playerSprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            playerSprite.color = playerColor;
            yield return new WaitForSeconds(flashDuration);
            currentFlash++;
        }
        //UpdateDamageBarUI(previousHealth);
        UpdateHealthBarUI();
        CheckIfPlayerHasDied();
        //_collider2D.enabled = true;
        hit = false;
    }

    protected override void OnCollided(GameObject collidedObject)
    {
        damageDealt = 0;
        if (collidedObject.GetComponent<EnemyBullet>() == null)
        {
            return;
        }

        EnemyBullet bullet = collidedObject.GetComponent<EnemyBullet>();

        if (bullet != null)
        {

            if (!(collidedObject.GetInstanceID() == lastBulletHitID))
            {
                lastBulletHitID = collidedObject.GetInstanceID();
                damageDealt = bullet.damage;
            }

            //damageDealt = bullet.damage;
        }
        else
        {
            damageDealt = collidedObject.GetComponent<Enemy>().contactDamage;
        }
        currentPlayerHealth -= damageDealt;
        UpdateHealthBarUI();
        StartCoroutine(FlashCoroutine());
        Debug.Log("Player hit with " + collidedObject.name);
    }

    public void ResetPlayerHealth()
    {
        UpdateHealthBarUI();
        this.currentPlayerHealth = defaultPlayerHealth;
    }

    public void HealPlayerHealtlh()
    {
        UpdateHealthBarUI();
        this.currentPlayerHealth = maxPlayerHealth;
    }

    public void CheckIfPlayerHasDied()
    {
        UpdateHealthBarUI();
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

    void UpdateDamageBarUI(int previousHealth)
    {
        // Method resets the bar and smoothly disappears 
        //LeanTween.value(gameObject, previousHealth, currentPlayerHealth, 3);
        //healthWhiteSlider.value = currentPlayerHealth;
        LeanTween.value(gameObject, previousHealth, currentPlayerHealth, 1).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float val) =>
        {
            healthWhiteSlider.value = val;
        });

        /*
        cdId = LeanTween.value(gameObject, healthWhiteSlider.maxValue, 0, healthWhiteSlider.maxValue).setEase(LeanTweenType.linear).setOnUpdate((float val) =>
        {
            cdSlider.value = val;
        }).setOnComplete(() =>
        {
            cdEnd();
        }).id;
        */

    }

    void UpdateHealthBarUI()
    {
        healthSlider.value = currentPlayerHealth;
        healthFillBar.GetComponent<Image>().color = heartColor;
    }

    void UpdateHealthBarUIDamaged(int previousHealth)
    {
        UpdateHealthBarUI();
        healthWhiteSlider.value = previousHealth;
    }
}
