using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // https://answers.unity.com/questions/855976/make-a-player-model-rotate-towards-mouse-location.html
    [SerializeField] private Camera mainCamera;
    private Vector2 mousePosition;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private SpriteRenderer playerSprite;
    public Sprite[] spriteList = new Sprite[6]; // 0 - Top left, 1 - Top, 2 - top right, 3 - bottom right, 4 - bottom, 5 - bottom left
    private float angle;
    public float offset;

    // Update is called once per frame
    void Update()
    {

        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        //Get the Screen position of the mouse
        mousePosition = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //Get the angle between the points
        angle = AngleBetweenTwoPoints(positionOnScreen, mousePosition);

        //Ta Daaa hard coding time woooo
        if (angle <= 0f && angle >= -60f)
        {
            // looking top left
            playerSprite.sprite = spriteList[0];
        }

        if (angle <= -60f && angle >= -120f)
        {
            // looking up
            playerSprite.sprite = spriteList[1];
        }

        if (angle <= -120f && angle >= -180f)
        {
            // looking top right
            playerSprite.sprite = spriteList[2];
        }

        if (angle <= 180f && angle >= 120f)
        {
            // looking bottom right
            playerSprite.sprite = spriteList[3];
        }

        if (angle <= 120f && angle >= 60f)
        {
            // looking bottom 
            playerSprite.sprite = spriteList[4];
        }

        if (angle <= 60f && angle >= 0f)
        {
            // looking bottom 
            playerSprite.sprite = spriteList[5];
        }
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

}
