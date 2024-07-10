using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager manager;

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

    void Awake()
    {
        if (manager == null)
        {
            manager = this;
        }
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

    private static void PlayerDeath()
    {

    }

}
