
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonSpawner", menuName = "DungeonSpawners/Spawner Data")]

public class DungeonSpawnerData : ScriptableObject
{
    public GameObject item;
    public int minSpawns;
    public int maxSpawns;
}
