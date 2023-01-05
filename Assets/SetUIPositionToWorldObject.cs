using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetUIPositionToWorldObject : MonoBehaviour
{
    // http://answers.unity3d.com/questions/799616/unity-46-beta-19-how-to-convert-from-world-space-t.html

    //this is your object that you want to have the UI element hovering over
    public GameObject targetWorldObject;
    //this is the ui element
    public RectTransform uiElement;

    public RectTransform canvasRect;

    public Camera mainCamera;
    public float offsetX;
    public float offsetY;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //then you calculate the position of the UI element
        //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

        if (!targetWorldObject.activeInHierarchy)
        {
            uiElement.gameObject.SetActive(false);
            return;
        }

        uiElement.gameObject.SetActive(true);
        Vector2 ViewportPosition = mainCamera.WorldToViewportPoint(targetWorldObject.transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)) + offsetX,
        ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)) + offsetY);

        //now you can set the position of the ui element
        uiElement.anchoredPosition = WorldObject_ScreenPosition;
    }
}
