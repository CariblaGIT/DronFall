using UnityEngine;

// Una recompensa posible: el item a instanciar (dejar vacio para representar "no cae nada") y su peso relativo frente al resto
[System.Serializable]
public struct WeightedReward
{
    public GameObject item;
    public float weight;
}

// Configuracion de las recompensas de una sala: una tabla de items con un peso cada uno; se sortea uno solo al limpiarla.
// Los pesos son relativos entre si (no hace falta que sumen 1): con pesos 60/40 el primero cae un 60% de las veces
[CreateAssetMenu(fileName = "DungeonRewardData", menuName = "DungeonSpawners/Reward Data")]
public class DungeonRewardData : ScriptableObject
{
    public WeightedReward[] possibleRewards;
}
