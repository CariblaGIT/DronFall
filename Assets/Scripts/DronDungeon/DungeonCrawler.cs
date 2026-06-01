using System.Collections.Generic;
using UnityEngine;

// Representa un "cavador" que se desplaza por la grid para ir marcando las posiciones donde habrá salas
public class DungeonCrawler : MonoBehaviour
{
    public Vector2Int Position { get; set;}

    public DungeonCrawler (Vector2Int initialPosition){
        Position = initialPosition;
    }

    // Elige una dirección al azar, avanza una posición en esa dirección y devuelve la nueva posición
    public Vector2Int Movement(Dictionary<PossibleDirections, Vector2Int> directionMovement){
        PossibleDirections move = (PossibleDirections)Random.Range(0, directionMovement.Count);
        Position += directionMovement[move];
        return Position;
    }
}
