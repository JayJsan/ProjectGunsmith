using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float cameraRange;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = (playerTransform.position + mousePos) / 2f;

        targetPos.x = Mathf.Clamp(targetPos.x, playerTransform.position.x - cameraRange, playerTransform.position.x + cameraRange);
        targetPos.y = Mathf.Clamp(targetPos.y, playerTransform.position.y - cameraRange, playerTransform.position.y + cameraRange);
        targetPos.z = 0f;

        this.transform.position = targetPos;
    }
}
