using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPivot : MonoBehaviour
{
    public GameObject enemy;
    public GameObject target;

    private void Start()
    {
        enemy = this.gameObject;
        target = GameObject.Find("Player");
    }

    private void FixedUpdate()
    {
        // Change mouse position to enemy aiming position
        Vector3 difference = target.GetComponent<Transform>().position - transform.position;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        if (rotationZ < -90 || rotationZ > 90)
        {
            // if player is looking right
            if (enemy.transform.eulerAngles.y == 0)
            {
                transform.localRotation = Quaternion.Euler(180f, 0f, -rotationZ);
            }
            else if (enemy.transform.eulerAngles.y == 180)
            {
                // if player is looking left
                transform.localRotation = Quaternion.Euler(180f, 180f, -rotationZ);
            }
        }
    }
}
