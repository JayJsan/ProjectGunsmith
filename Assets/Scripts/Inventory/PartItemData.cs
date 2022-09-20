using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gun Part Item Data")]
public class PartItemData : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite partSprite;
    public GameObject prefab;
}
