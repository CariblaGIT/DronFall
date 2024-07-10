using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PossibleDirections{
    top = 0,
    left = 1,
    bottom = 2,
    right = 3
};

public class DungeonCrawlerController : MonoBehaviour
{
    public static List<Vector2Int> visitedPositions = new List<Vector2Int>();
    private static readonly Dictionary<PossibleDirections, Vector2Int> possibleDirectionsMap = new Dictionary<PossibleDirections, Vector2Int>{
        {PossibleDirections.top, Vector2Int.up},
        {PossibleDirections.left, Vector2Int.left},
        {PossibleDirections.bottom, Vector2Int.down},
        {PossibleDirections.right, Vector2Int.right}
    };

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
