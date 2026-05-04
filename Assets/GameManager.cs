using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static int healthPoints = 10;
    private static int maxHealthPoints = 10;
    private static float speedMovement = 5f;
    private static float shootRange = 2f;
    private static float shootFire = 7f;
    private static float shootSize = 1f;

    public static int Health
    {
        get => healthPoints;
        set => healthPoints = value;
    }

    public static int MaxHealth
    {
        get => maxHealthPoints;
        set => maxHealthPoints = value;
    }

    public static float MovementSpeed
    {
        get => speedMovement;
        set => speedMovement = value;
    }

    public static float FireSpeed
    {
        get => shootFire;
        set => shootFire = value;
    }

    public static float FireSize
    {
        get => shootSize;
        set => shootSize = value;
    }

    public static void PlayerDamage(int hit)
    {
        healthPoints -= hit;
        if(healthPoints <= 0)
        {
            PlayerDeath();
        }
    }

    public static void PlayerHeal(int heal)
    {
        if(healthPoints < maxHealthPoints)
        {
            healthPoints = Mathf.Min(maxHealthPoints, healthPoints + heal);
        }
    }

    public static void PlayerIncreaseSpeed(float speed)
    {
        speedMovement += speed;
    }

    public static void PlayerIncreaseRange(float range)
    {
        shootRange += range;
    }

    public static void PlayerIncreaseAttackSpeed(float attackSpeed)
    {
        shootFire -= attackSpeed;
    }

    public static void PlayerIncreaseShoot(float bulletSize)
    {
        shootSize += bulletSize;
    }

    // Se dispara cuando la vida llega a 0; GameOverController se suscribe para mostrar la pantalla de derrota
    public static event System.Action OnPlayerDeath;

    private static void PlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }

    // Se dispara al destruir el nucleo del ultimo nivel; VictoryController se suscribe para mostrar la pantalla de victoria
    public static event System.Action OnVictory;

    public static void TriggerVictory()
    {
        OnVictory?.Invoke();
    }

    // Devuelve el estado del jugador a los valores iniciales; se llama al empezar una partida nueva
    // o al volver a la pantalla de titulo, para no arrastrar las mejoras de la partida anterior
    public static void ResetState()
    {
        healthPoints = 10;
        maxHealthPoints = 10;
        speedMovement = 5f;
        shootRange = 2f;
        shootFire = 7f;
        shootSize = 1f;
    }

}
