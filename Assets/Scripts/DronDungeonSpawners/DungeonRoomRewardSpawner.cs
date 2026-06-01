using UnityEngine;

// Se coloca en la sala junto a DungeonObjectRoomSpawner; el DungeonRoomController lo invoca la primera vez
// que la sala se queda sin enemigos vivos, para intentar dar una recompensa en el centro de la sala
public class DungeonRoomRewardSpawner : MonoBehaviour
{
    public DungeonRoom dungeonRoom;
    public DungeonRewardData rewardData;

    void Awake()
    {
        if(dungeonRoom == null)
        {
            dungeonRoom = GetComponentInParent<DungeonRoom>();
        }
    }

    // Sortea una recompensa de la tabla segun su peso relativo y la instancia en el centro de la sala (si no sale vacia)
    public void TrySpawnReward()
    {
        GameObject rewardItem = PickWeightedReward();
        if(rewardItem != null)
        {
            Instantiate(rewardItem, dungeonRoom.GetRoomCentre(), Quaternion.identity, transform);
        }
    }

    // Suma los pesos de todas las recompensas y sortea un punto dentro de ese total para elegir cual toca
    private GameObject PickWeightedReward()
    {
        if(rewardData == null || rewardData.possibleRewards.Length == 0)
        {
            return null;
        }

        float totalWeight = 0f;
        foreach (WeightedReward reward in rewardData.possibleRewards)
        {
            totalWeight += reward.weight;
        }

        if(totalWeight <= 0f)
        {
            return null;
        }

        float roll = Random.Range(0f, totalWeight);
        float accumulated = 0f;
        foreach (WeightedReward reward in rewardData.possibleRewards)
        {
            accumulated += reward.weight;
            if(roll <= accumulated)
            {
                return reward.item;
            }
        }

        return null;
    }
}
