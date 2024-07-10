using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonObjectRoomSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct RandomRoomSpawner
    {
        public string typeSpawner;
        public DungeonSpawnerData spawnerData;

    }

    public DungeonGridController dungeonGrid;
    public RandomRoomSpawner[] spawnerData;

    void Start()
    {
        //dungeonGrid = GetComponentInChildren<DungeonGridController>();
    }

    public void StartSpawning(){
        foreach (RandomRoomSpawner spawner in spawnerData)
        {
            DungeonSpawns(spawner);
        }
    }

    void DungeonSpawns(RandomRoomSpawner spawner){
        int rand = Random.Range(spawner.spawnerData.minSpawns, spawner.spawnerData.maxSpawns + 1);
        for (int i = 0; i < rand; i++)
        {
            int randPosition = Random.Range(0, dungeonGrid.pointsAvailable.Count - 1);
            GameObject spawn = Instantiate(spawner.spawnerData.item, dungeonGrid.pointsAvailable[randPosition], Quaternion.identity, transform) as GameObject;
            dungeonGrid.pointsAvailable.RemoveAt(randPosition);
        }
    }
}
