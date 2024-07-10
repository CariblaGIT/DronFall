
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonGenerationData", menuName = "DungeonGenerationData/Dungeon Data")]
public class DungeonGenerationData : ScriptableObject {
    public int crawlersQuantity;
    public int minIterations;
    public int maxIterations;
}