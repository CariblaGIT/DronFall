
using UnityEngine;

// Configuración del algoritmo de generación: cuántos crawlers usar y entre cuántas iteraciones se mueven
[CreateAssetMenu(fileName = "DungeonGenerationData", menuName = "DungeonGenerationData/Dungeon Data")]
public class DungeonGenerationData : ScriptableObject {
    public int crawlersQuantity;
    public int minIterations;
    public int maxIterations;
}