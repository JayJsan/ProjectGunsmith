using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    #region COMPONENTS
    public GameObject firePoint;
    #endregion

    #region Gun Stat Variables
    public int damage = 1;
    public float Range = 10f;
    #endregion
    private void Start()
    {
        if (GameObject.Find("Enemy/GunPivot/EnemyFirePoint") != null)
        {
            firePoint = GameObject.Find("Enemy/GunPivot/EnemyFirePoint");
        }
    }

    void FixedUpdate()
    {
        if (firePoint != null)
        {
            float distanceDelta = (transform.position - firePoint.transform.position).magnitude;
            if (distanceDelta > Range)
            {
                //StartCoroutine(DestroyBullet());
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator DestroyBullet()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo != null)
        {
            if (hitInfo.tag.Equals("Shootable") || hitInfo.tag.Equals("Player"))
            {
                Debug.Log(hitInfo.name + " was hit!");
                //StartCoroutine(DestroyBullet());
                Destroy(gameObject, 1f);
            }
        }
    }
}
