
using UnityEngine;

// Configuración de un tipo de spawn: qué prefab instanciar y entre cuántas veces (mínimo/máximo) por sala
[CreateAssetMenu(fileName = "DungeonSpawner", menuName = "DungeonSpawners/Spawner Data")]

public class DungeonSpawnerData : ScriptableObject
{
    public GameObject item;
    public int minSpawns;
    public int maxSpawns;
}
