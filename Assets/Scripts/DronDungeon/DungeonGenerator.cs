using System.Collections.Generic;
using UnityEngine;

// Punto de entrada de la generación de un nivel: pide las posiciones de las salas y las encola para cargarlas
public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData dungeonData;

    private void Start(){
        List<Vector2Int> dungeonRoomsPositions = DungeonCrawlerController.DungeonGeneration(dungeonData);
        SpawnRoomsIntoMap(dungeonRoomsPositions);
    }

    // Encola la sala inicial en (0,0) y una sala genérica por cada posición generada por los crawlers
    private void SpawnRoomsIntoMap(IEnumerable<Vector2Int> dungeonRooms){
        DungeonRoomController.controllerInstance.LoadDungeonRoom("Start", 0 ,0);
        foreach(Vector2Int dungeonRoomLocation in dungeonRooms){
            DungeonRoomController.controllerInstance.LoadDungeonRoom("Empty", dungeonRoomLocation.x , dungeonRoomLocation.y);
        }
    }
}
