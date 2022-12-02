using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    /**
     *  Code used from SixDot's video "Procedural Generation in Unity"
     *  https://www.youtube.com/watch?v=nADIYwgKHv4
     */

    Vector2 worldSize = new Vector2(4, 4);
    Room[,] rooms;
    List<Vector2> takenPositions = new List<Vector2>();

    int gridSizeX, gridSizeY, numberOfRooms = 20;

    public GameObject roomWhiteObj;

    private void Start()
    {
        if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
        {
            numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
        }
        gridSizeX = Mathf.RoundToInt(worldSize.x);
        gridSizeY = Mathf.RoundToInt(worldSize.y);
        CreateRooms();
        SetRoomDoors();
        DrawMap();
    }

    void CreateRooms()
    {
        // Setup
        rooms = new Room[gridSizeX * 2, gridSizeY * 2];
        rooms[gridSizeX, gridSizeY] = new Room(Vector2.zero, 1);
        takenPositions.Insert(0, Vector2.zero);
        Vector2 checkPos = Vector2.zero;
        // magic numbers???
        float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
        // Add rooms
        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            float randomPerc = ((float)i) / (((float)numberOfRooms - 1));
            randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
            // grab new position
            checkPos = NewPosition();
            // test new position ----- this block of code makes up how the level is shaped
            if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare)
            {
                int iterations = 0;
                do
                {
                    checkPos = SelectiveNewPosition();
                    iterations++;
                } while (NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);

                if (iterations >= 50)
                {
                    print("error: could not create with fewer neighbors than: " + NumberOfNeighbors(checkPos, takenPositions));
                }
                // finalise position
                rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new Room(checkPos, 0);
                takenPositions.Insert(0, checkPos);
            }
        }
    }

    void SetRoomDoors()
    {
        for (int x = 0; x < ((gridSizeX* 2)); x++)
        {
            for (int y = 0; y < ((gridSizeY * 2)); y++)
            {
                if (rooms[x,y] == null)
                {
                    continue;
                }
                Vector2 gridPosition = new Vector2(x, y);

                if (y - 1 < 0) 
                { // Check Above
                    rooms[x, y].doorBot = false;
                } else
                {
                    rooms[x, y].doorBot = (rooms[x, y - 1] != null);
                }

                if (y + 1 >= gridSizeY * 2)
                { // Check below
                    rooms[x, y].doorTop = false;
                } else
                {
                    rooms[x, y].doorTop = (rooms[x, y + 1] != null);
                }

                if (x - 1 < 0)
                { // Check left
                    rooms[x, y].doorLeft = false;
                }
                else
                {
                    rooms[x, y].doorLeft = (rooms[x - 1, y] != null);
                }

                if (x + 1 >= gridSizeX * 2)
                { // Check right
                    rooms[x, y].doorRight = false;
                } else
                {
                    rooms[x, y].doorRight = (rooms[x + 1, y] != null);
                }
            }
        }
    }

    void DrawMap()
    {

    }

    Vector2 NewPosition()
    {
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;
        do
        {
            int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (UpDown)
            {
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        return checkingPos;
    }

    int NumberOfNeighbors(Vector2 checkPos, List<Vector2> usedPositions)
    {
        int ret = 0;
        if (usedPositions.Contains(checkPos + Vector2.right))
        {
            ret++;
        }
        if (usedPositions.Contains(checkPos + Vector2.left))
        {
            ret++;
        }
        if (usedPositions.Contains(checkPos + Vector2.up))
        {
            ret++;
        }
        if (usedPositions.Contains(checkPos + Vector2.down))
        {
            ret++;
        }
        return ret;
    }

    Vector2 SelectiveNewPosition()
    {
        // not implemented
        return Vector2.zero;
    }
}
