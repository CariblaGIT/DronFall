using System.Collections.Generic;
using UnityEngine;

// Genera una grid de puntos dentro de una sala para poder posicionar en ellos enemigos, ítems, etc.
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

    // GameObject que funciona como celda para visualizar la grid durante el desarrollo
    public GameObject dungeonGridTile;

    // Lista con los puntos disponibles para hacer spawn de cosas en la grid
    public List<Vector2> pointsAvailable = new List<Vector2>();

    // Configura las variables del struct de la grid a partir de la sala y empieza a generarla
    void Awake()
    {
        dungeonRoom = GetComponentInParent<DungeonRoom>();
        dungeonGrid.columns = dungeonRoom.DungeonRoomWidth - 2;
        dungeonGrid.rows = dungeonRoom.DungeonRoomHeight - 2;
        dungeonGrid.verticalOff = dungeonRoom.transform.localPosition.y;
        dungeonGrid.horizontalOff = dungeonRoom.transform.localPosition.x;
        GenerateGrid();
    }

    /* Genera una grid dentro de una sala con puntos en los que más tarde se hará spawn de enemigos, ítems y demás,
       usando el método StartSpawning de DungeonObjectRoomSpawner */
    public void GenerateGrid()
    {
        for (int y = 0; y < dungeonGrid.rows; y++)
        {
            for (int x = 0; x < dungeonGrid.columns; x++)
            {
                GameObject gridTile = Instantiate(dungeonGridTile, transform);
                gridTile.transform.position = new Vector2(x - (dungeonGrid.columns / 2) + dungeonGrid.horizontalOff, y - (dungeonGrid.rows / 2) + dungeonGrid.verticalOff);
                gridTile.name = "X => " + x + " // Y => " + y;
                pointsAvailable.Add(gridTile.transform.position);
                gridTile.SetActive(false);
            }
        }
        GetComponentInParent<DungeonObjectRoomSpawner>().StartSpawning();
    }
}

