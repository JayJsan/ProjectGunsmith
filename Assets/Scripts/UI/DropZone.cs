using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    public bool hoveringOver;
    public DragAndDrop.partTypes zoneType;
    public BoxCollider2D dropZoneCollider;
    public bool isOccupied;
    public GameObject OccupyingGunPart;

    private void Start()
    {
        dropZoneCollider = GetComponent<BoxCollider2D>();
        if (GetComponent<SpriteRenderer>() != null)
        {
            //GetComponent<SpriteRenderer>().enabled = false;
        }
        // Checks the name of the zoneholder and sees if it matches any of the enums (magazine)
        // and sets the zonetype to that part type.
        foreach (DragAndDrop.partTypes partType in System.Enum.GetValues(typeof(DragAndDrop.partTypes)))
        {
            if (this.gameObject.name.Contains(partType.ToString()))
            {
                zoneType = partType;
            }
        }

    }
    private void OnMouseOver()
    {
        hoveringOver = true;
    }

    private void OnMouseExit()
    {
        hoveringOver = false;
    }

    public void DropGunPart()
    {
        DragAndDrop dragAndDrop = OccupyingGunPart.GetComponent<DragAndDrop>();
        if (dragAndDrop != null)
        {
            //dragAndDrop. 
        }
    }
}
