using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    /**
     * Code used from SixDot's video "Procedural Generation in Unity"
     * https://www.youtube.com/watch?v=nADIYwgKHv4
     */
    public Vector2 gridPos;
    public int type;
    public bool doorTop, doorBot, doorLeft, doorRight;
    
    public Room(Vector2 _gridPos, int _type)
    {
        gridPos = _gridPos;
        type = _type;
    }
}
