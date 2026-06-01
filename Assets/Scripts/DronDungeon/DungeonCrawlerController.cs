using System.Collections.Generic;
using UnityEngine;

// Direcciones posibles en las que se puede mover un crawler
public enum PossibleDirections{
    top = 0,
    left = 1,
    bottom = 2,
    right = 3
};

// Controla el algoritmo de generación de la mazmorra: mueve crawlers al azar y acumula las posiciones visitadas
public class DungeonCrawlerController : MonoBehaviour
{
    // Posiciones visitadas por los crawlers; se usan luego para saber dónde cargar cada sala
    public static List<Vector2Int> visitedPositions = new List<Vector2Int>();

    // Traduce cada dirección posible al desplazamiento en la grid que representa
    private static readonly Dictionary<PossibleDirections, Vector2Int> possibleDirectionsMap = new Dictionary<PossibleDirections, Vector2Int>{
        {PossibleDirections.top, Vector2Int.up},
        {PossibleDirections.left, Vector2Int.left},
        {PossibleDirections.bottom, Vector2Int.down},
        {PossibleDirections.right, Vector2Int.right}
    };

    // Genera la mazmorra moviendo los crawlers un número aleatorio de iteraciones (entre minIterations y maxIterations)
    public static List<Vector2Int> DungeonGeneration(DungeonGenerationData data){
        List<DungeonCrawler> crawlers = new List<DungeonCrawler>();
        DungeonCrawler crawZero = new DungeonCrawler(Vector2Int.zero);
        for(int i = 0; i < data.crawlersQuantity; i++){
            crawlers.Add(crawZero);
        }
        int loops = Random.Range(data.minIterations, data.maxIterations);
        for(int i = 0; i < loops; i++){
            foreach (DungeonCrawler crawler in crawlers)
            {
                Vector2Int newPosition = crawler.Movement(possibleDirectionsMap);
                visitedPositions.Add(newPosition);
            }
        }

        return visitedPositions;
    }
}
