using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData dungeonData;
    private List<Vector2Int> listDungeonRooms;
    private void Start(){
        listDungeonRooms = DungeonCrawlerController.DungeonGeneration(dungeonData);
        SpawnRoomsIntoMap(listDungeonRooms);
    }
    private void SpawnRoomsIntoMap(IEnumerable<Vector2Int> dungeonRooms){
        DungeonRoomController.controllerInstance.LoadDungeonRoom("Start", 0 ,0);
        foreach(Vector2Int dungeonRoomLocation in dungeonRooms){
            DungeonRoomController.controllerInstance.LoadDungeonRoom("Empty", dungeonRoomLocation.x , dungeonRoomLocation.y);
        }
    }
}
