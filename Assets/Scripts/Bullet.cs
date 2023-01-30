using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region COMPONENTS
    public GameObject firePoint;
    #endregion

    #region Gun Stat Variables
    public int damage = 25;
    public int piercing = 1;
    private int amountPierced = 0;
    public float Range = 10f;

    public int lastEnemyHit = 0;
    #endregion

    private void Start()
    {
        firePoint = GameObject.FindWithTag("FirePoint");

    }

    private void OnEnable()
    {
        amountPierced = 0;
        lastEnemyHit = 0;
    }

    void FixedUpdate()
    {
        float distanceDelta = (transform.position - firePoint.transform.position).magnitude;
        if (distanceDelta > Range)
        {
            //Destroy(gameObject); 
            gameObject.SetActive(false); // FOR OUR GAMEOBJECT POOLIGN SYSTEM
        }
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag.Equals("Shootable") || hitInfo.tag.Equals("Enemy"))
        {
            Debug.Log(hitInfo.name + " was hit!");
            Enemy enemy = null;

            if (hitInfo.GetComponent<Enemy>() != null)
            {
                enemy = hitInfo.GetComponent<Enemy>();
            }

            if (enemy != null)
            {

                if (enemy.GetInstanceID() != lastEnemyHit)
                {
                    amountPierced++;
                    enemy.TakeDamage(damage);
                    lastEnemyHit = enemy.GetInstanceID();
                }
            }
            //Destroy(gameObject);
            if (amountPierced >= piercing)
            {
                gameObject.SetActive(false); // FOR OUR GAMEOBJECT POOLIGN SYSTEM
            }
        }
    }
}
