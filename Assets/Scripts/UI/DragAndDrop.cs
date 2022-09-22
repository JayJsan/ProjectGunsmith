using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging;

    [SerializeField]
    private DropZone dz;
    private Shooting playerShooting;

    #region DROP ZONES
    private DropZone barrelZone;
    private DropZone magazineZone;
    private DropZone triggerZone;
    private DropZone sightZone;
    private DropZone stockZone;
    private DropZone specialZone;
    #endregion

    public string partType;

    private void Start()
    {
        #region DROP ZONE GET COMPONENT
        barrelZone = GameObject.Find("CM vcam1/WeaponHolder/BarrelHolder").GetComponent<DropZone>();
        magazineZone = GameObject.Find("CM vcam1/WeaponHolder/MagazineHolder").GetComponent<DropZone>();
        triggerZone = GameObject.Find("CM vcam1/WeaponHolder/TriggerHolder").GetComponent<DropZone>();
        sightZone = GameObject.Find("CM vcam1/WeaponHolder/SightHolder").GetComponent<DropZone>();
        stockZone = GameObject.Find("CM vcam1/WeaponHolder/StockHolder").GetComponent<DropZone>();
        specialZone = GameObject.Find("CM vcam1/WeaponHolder/SpecialHolder").GetComponent<DropZone>();

        #endregion

        dz = GameObject.FindWithTag("DropZone").GetComponent<DropZone>();
        playerShooting = GameObject.Find("Player").GetComponent<Shooting>();
        partType = this.gameObject.name;
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
        if (!dz.hoveringOver)
        {
            transform.localPosition = Vector3.zero;
        }

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
}
