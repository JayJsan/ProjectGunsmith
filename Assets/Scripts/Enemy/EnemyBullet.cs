using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    #region COMPONENTS
    public GameObject firePoint;
    private SpriteRenderer bulletSprite;
    #endregion

    #region Gun Stat Variables
    public int damage = 1;
    public float Range = 10f;
    #endregion
    private void Awake()
    {
        bulletSprite = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if (GameObject.Find("Enemy/GunPivot/EnemyFirePoint") != null)
        {
            firePoint = GameObject.Find("Enemy/GunPivot/EnemyFirePoint");
        }

    }

    private void OnEnable()
    {
        bulletSprite.color = new Color(1f, 1f, 1f, 1f);
    }

    void FixedUpdate()
    {
        if (firePoint != null)
        {
            float distanceDelta = (transform.position - firePoint.transform.position).magnitude;
            if (distanceDelta > Range)
            {
                //StartCoroutine(DestroyBullet());
                //Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator DestroyBullet()
    {
        bulletSprite.color = new Color(1f, 1f, 1f, 0f);
        yield return new WaitForSeconds(0.01f);
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo != null)
        {
            if (hitInfo.tag.Equals("Player"))
            {
                Debug.Log(hitInfo.name + " was hit!");
                hitInfo.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
                StartCoroutine(DestroyBullet());
                //Destroy(gameObject, 1f);
            }
            else if (hitInfo.tag.Equals("Shootable"))
            {
                Debug.Log(hitInfo.name + " was hit!");
                StartCoroutine(DestroyBullet());
            }
        }
    }
}
