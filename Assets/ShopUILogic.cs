using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
public class ShopUILogic : MonoBehaviour
{
    // http://answers.unity3d.com/questions/799616/unity-46-beta-19-how-to-convert-from-world-space-t.html

    //this is your object that you want to have the UI element hovering over
    public GameObject targetWorldObject;
    //this is the ui element
    public RectTransform uiElement;
    public TextMeshProUGUI uiStatsText;
    public TextMeshProUGUI uiCostText;
    public TextMeshProUGUI uiGoldText;

    public Color goldTextColor;
    public Color goldFlashColor;
    public float flashDuration;
    public RectTransform canvasRect;

    public Camera mainCamera;
    public float offsetX;
    public float offsetY;

    public bool isHovering;

    private void Start()
    {
        //first you need the RectTransform component of your canvas
        canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
    }

    void Update()
    {
        if (isHovering)
        {
            if (targetWorldObject == null)
            {
                return;
            }


            UpdateStatsUI();
            uiElement.gameObject.SetActive(true);
            //then you calculate the position of the UI element
            //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

            Vector2 ViewportPosition = mainCamera.WorldToViewportPoint(targetWorldObject.transform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)) + offsetX,
            ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)) + offsetY);

            //now you can set the position of the ui element
            uiElement.anchoredPosition = WorldObject_ScreenPosition;
        } else
        {
            uiElement.gameObject.SetActive(false);
        }
    }

    public void PlayerIsHovering()
    {
        isHovering = true;
    }

    public void PlayerIsntHovering()
    {
        isHovering = false;
    }

    public void ChangeUIHoverTarget(GameObject newTarget)
    {
        targetWorldObject = newTarget;
    }

    private IEnumerator GoldFlashCoroutine()
    {
        int currentFlash = 0;
        int numberOfFlashes = 3;
        while (currentFlash < numberOfFlashes)
        {
            uiGoldText.color = goldFlashColor;
            yield return new WaitForSeconds(flashDuration);
            uiGoldText.color = goldTextColor;
            yield return new WaitForSeconds(flashDuration);
            currentFlash++;
        }
    }

    public void PlayerIsBroke()
    {
        StartCoroutine(GoldFlashCoroutine());
    }

    void UpdateStatsUI()
    {
        PartItemData gunPartStats = targetWorldObject.GetComponent<InteractableGunPart>().gunPartData;

        string TriggerType;
        if (gunPartStats.displayName.Contains("Trigger"))
        {
            if (gunPartStats.isAuto)
            {
                TriggerType = "Auto";
            }
            else
            {
                TriggerType = "Semi-Auto";
            }
        } else
        {
            TriggerType = "n/a";
        }
       

        uiStatsText.text = string.Format("Name: {0}\r\n" +
            "Bullet Speed: {1}\r\n" +
            "Accuracy Multiplier: {2}%\r\n" +
            "Damage: {3}\r\n" +
            "Range: {4}\r\n" +
            "Fire Rate Multiplier: {5}x\r\n" +
            "Max Ammo: {6}\r\n" +
            "Bullets per shot: {7}\r\n" +
            "Trigger Type: {8}",
            gunPartStats.displayName,
            gunPartStats.bulletForce,
            gunPartStats.accuracyMultiplier * 100,
            gunPartStats.damage,
            gunPartStats.range,
            gunPartStats.fireRateMultiplier,
            gunPartStats.maxAmmo,
            gunPartStats.numberOfBullets,
            TriggerType);

        uiCostText.text = string.Format("Cost: " + gunPartStats.cost + "$");

    }
}
