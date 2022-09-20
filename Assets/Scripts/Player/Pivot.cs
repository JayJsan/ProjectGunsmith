using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour
{
    public GameObject myPlayer;
    public Transform gunModelTransform;

    private void FixedUpdate()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f,0f,rotationZ);

        if (rotationZ < -90 || rotationZ > 90)
        {
            // if player is looking right
            if (myPlayer.transform.eulerAngles.y == 0)
            {
                transform.localRotation = Quaternion.Euler(180f, 0f, -rotationZ);
            } else if (myPlayer.transform.eulerAngles.y == 180)
            {
                // if player is looking left
                transform.localRotation = Quaternion.Euler(180f,180f,-rotationZ);
            }
        }
    }
}
