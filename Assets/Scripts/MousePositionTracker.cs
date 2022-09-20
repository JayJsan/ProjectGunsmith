using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionTracker : MonoBehaviour
{
    public Transform playerPosition;
    public Vector3 playerPos;
    public Vector3 mousePosition;
    public Vector3 cameraPosition;
    public float rangeCap = 2f;
    // Update is called once per frame

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        cameraPosition.x = Mathf.Clamp(mousePosition.x, playerPosition.position.x, playerPosition.position.x + rangeCap);
        cameraPosition.y = Mathf.Clamp(mousePosition.y, playerPosition.position.y, playerPosition.position.y + rangeCap);


        //transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f);
        transform.position = cameraPosition;    }

}
