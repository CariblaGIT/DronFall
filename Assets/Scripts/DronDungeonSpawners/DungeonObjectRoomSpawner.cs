using UnityEngine;

// Se encarga de instanciar enemigos/ítems dentro de una sala, repartidos en los puntos libres de su DungeonGridController
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

    // Lanza el spawn de todos los tipos configurados; lo llama DungeonGridController cuando ya ha generado su grid
    public void StartSpawning(){
        foreach (RandomRoomSpawner spawner in spawnerData)
        {
            DungeonSpawns(spawner);
        }
    }

    // Instancia una cantidad aleatoria (entre minSpawns y maxSpawns) del prefab del spawner en puntos aleatorios de la grid
    void DungeonSpawns(RandomRoomSpawner spawner){
        int rand = Random.Range(spawner.spawnerData.minSpawns, spawner.spawnerData.maxSpawns + 1);
        for (int i = 0; i < rand; i++)
        {
            int randPosition = Random.Range(0, dungeonGrid.pointsAvailable.Count - 1);
            GameObject spawn = Instantiate(spawner.spawnerData.item, dungeonGrid.pointsAvailable[randPosition], Quaternion.identity, transform);
            dungeonGrid.pointsAvailable.RemoveAt(randPosition);
        }
    }
}
