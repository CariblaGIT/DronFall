using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCrawler : MonoBehaviour
{
    public Vector2Int Position { get; set;}

    public DungeonCrawler (Vector2Int initialPosition){
        Position = initialPosition;
    }

    public Vector2Int Movement(Dictionary<PossibleDirections, Vector2Int> directionMovement){
        PossibleDirections move = (PossibleDirections)Random.Range(0, directionMovement.Count);
        Position += directionMovement[move];
        return Position;
    }
}
