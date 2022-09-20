using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    public bool hoveringOver;
    public string holderName;

    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        holderName = this.gameObject.name;
    }
    private void OnMouseOver()
    {
        hoveringOver = true;
    }

    private void OnMouseExit()
    {
        hoveringOver = false;
    }
}
