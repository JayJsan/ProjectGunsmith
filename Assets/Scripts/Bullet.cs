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
    #endregion

    private void Start()
    {
        firePoint = GameObject.FindWithTag("FirePoint");
        amountPierced = 0;
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
                amountPierced++;
                enemy.TakeDamage(damage);
            }
            //Destroy(gameObject);
            if (amountPierced >= piercing)
            {
                gameObject.SetActive(false); // FOR OUR GAMEOBJECT POOLIGN SYSTEM
            }
        }
    }
}
