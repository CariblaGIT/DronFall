using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGridController : MonoBehaviour
{
    public DungeonRoom dungeonRoom;

    [System.Serializable]
    public struct DungeonGrid
    {
        public int rows;
        public int columns;
        public float verticalOff;
        public float horizontalOff;
    }

    public DungeonGrid dungeonGrid;

    // GameObject that works as a cell to draw on developing
    public GameObject dungeonGridTile;

    // List with the possible points for spawning things on the grid
    public List<Vector2> pointsAvailable = new List<Vector2>();

    // Awake method to setup the variables of the struct of the grid and start generating a Grid
    void Awake()
    {
        dungeonRoom = GetComponentInParent<DungeonRoom>();
        dungeonGrid.columns = dungeonRoom.DungeonRoomWidth - 2;
        dungeonGrid.rows = dungeonRoom.DungeonRoomHeight - 2;
        dungeonGrid.verticalOff = dungeonRoom.transform.localPosition.y;
        dungeonGrid.horizontalOff = dungeonRoom.transform.localPosition.x;
        GenerateGrid();
    }

    /* Method to generate a Grid inside a room to set points in which will spawn enemies, pickups and other stuff
       after they are generated using the StartSpawning method from the DungeonObjectRoomSpawner */
    public void GenerateGrid()
    {
        for (int y = 0; y < dungeonGrid.rows; y++)
        {
            for (int x = 0; x < dungeonGrid.columns; x++)
            {
                GameObject gridTile = Instantiate(dungeonGridTile, transform);
                gridTile.GetComponent<Transform>().position = new Vector2(x - (dungeonGrid.columns / 2) + dungeonGrid.horizontalOff, y - (dungeonGrid.rows / 2) + dungeonGrid.verticalOff);
                gridTile.name = "X => " + x + " // Y => " + y;
                pointsAvailable.Add(gridTile.transform.position);
                gridTile.SetActive(false);
            }
        }
        GetComponentInParent<DungeonObjectRoomSpawner>().StartSpawning();
    }
}

