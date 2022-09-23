using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public enum partTypes { Barrel, Magazine, Trigger, Sight, Stock, Special}
    private bool isDragging;

    [SerializeField]
    private DropZone dz;
    private Shooting playerShooting;

    #region DROP ZONES
    public DropZone barrelZone;
    public DropZone magazineZone;
    public DropZone triggerZone;
    public DropZone sightZone;
    public DropZone stockZone;
    public DropZone specialZone;
    #endregion

    public partTypes partType;
    private void Start()
    {
        #region DROP ZONE GET COMPONENT
        barrelZone = GameObject.Find("CMvcam1/WeaponHolder/BarrelHolder").GetComponent<DropZone>();
        magazineZone = GameObject.Find("CMvcam1/WeaponHolder/MagazineHolder").GetComponent<DropZone>();
        triggerZone = GameObject.Find("CMvcam1/WeaponHolder/TriggerHolder").GetComponent<DropZone>();
        sightZone = GameObject.Find("CMvcam1/WeaponHolder/SightHolder").GetComponent<DropZone>();
        stockZone = GameObject.Find("CMvcam1/WeaponHolder/StockHolder").GetComponent<DropZone>();
        specialZone = GameObject.Find("CMvcam1/WeaponHolder/SpecialHolder").GetComponent<DropZone>();

        #endregion

        dz = GameObject.FindWithTag("DropZone").GetComponent<DropZone>();
        playerShooting = GameObject.Find("Player").GetComponent<Shooting>();
        foreach (DragAndDrop.partTypes partType in System.Enum.GetValues(typeof(DragAndDrop.partTypes)))
        {
            if (this.gameObject.name.Contains(partType.ToString()))
            {
                this.partType = partType;
            }
        }
        playerShooting.enableShoot = true;
    }


    public void OnMouseDown()
    {
        isDragging = true;
        dz.GetComponent<SpriteRenderer>().enabled = true;
        playerShooting.enableShoot = false;
    }

    public void OnMouseUp()
    {
        isDragging = false;
        dz.GetComponent<SpriteRenderer>().enabled = false;
        playerShooting.enableShoot = true;
        // If player is hovering over drop zone, remove gunparts ability to return to gun holder and drop
        /* locks sprite
        if (!dz.hoveringOver)
        {
            transform.localPosition = Vector3.zero;
        }

        */
        CheckGunType();


    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
        }
    }

    void CheckGunType()
    {
        // Check what type of drop zone gun part is hovering over.
        // If drop zone type == gun part type, lock position to respective drop zone
        switch (partType)
        {
            case partTypes.Barrel:
                if (barrelZone.hoveringOver)
                {
                    transform.localPosition = Vector3.zero;
                }
                break;
            case partTypes.Magazine:
                if (magazineZone.hoveringOver)
                {
                    transform.localPosition = Vector3.zero;
                }
                break;
            case partTypes.Sight:
                if (sightZone.hoveringOver)
                {
                    transform.localPosition = Vector3.zero;
                }
                break;
            case partTypes.Special:
                if (specialZone.hoveringOver)
                {
                    transform.localPosition = Vector3.zero;
                }
                break;
            case partTypes.Stock:
                if (stockZone.hoveringOver)
                {
                    transform.localPosition = Vector3.zero;
                }
                break;
            case partTypes.Trigger:
                if (triggerZone.hoveringOver)
                {
                    transform.localPosition = Vector3.zero; 
                }
                break;
        }


        // If there is an existing gun part, drop existing gun part and lock new gun part onto gun part holder
        // If gun part hovering over actual drop zone then unlock gun part from position and drop gun part
        // Use switch case(?)
        
        // Gun part should be a prefab(?) - scale down when dropped and scale up when picked up
        // Gun part should have a reference to a scriptable object with its respective stats
        // Hovering gun part in its dropped state should bring up a tooltip
        // Picking it up should disable tooltip and scale to original size

        // Statmanager should check every gun part holder for its stats
        // Change each weapons stats accordingly
        // Gun sprite should change accordingly
    }
    void DropGunPart(partTypes zoneType)
    {

    }
}
