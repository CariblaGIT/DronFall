using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Transactions;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Follow,
    Attack,
    Death
};

public enum EnemyType
{
    BasicDron,
    KamikazeDron,
    MeleeDron,
    CargoDron

}

public class EnemyController : MonoBehaviour
{
    GameObject player;
    public EnemyState actualState = EnemyState.Idle;
    public EnemyType enemyType;
    public float enemyRange;
    public float enemyCooldown;
    public float enemyAttackRange;
    public int enemyDamage;
    public float enemySpeed;
    public float enemyShootSpeed;
    private bool enemyCooldownAttack = false;
    public bool notInRoom = false;
    private bool isDeath = false;
    private bool selectDirection = false;
    private Vector3 randMovementDirection;
    public GameObject bulletPrefab;

    private bool PlayerInEnemyRange(float range)
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    

    private IEnumerator SelectDirection()
    {
        selectDirection = true;
        yield return new WaitForSeconds(Random.Range(2f, 8f));
        randMovementDirection = new Vector3(0, 0, Random.Range(0, 360));
        Quaternion nextDirection = Quaternion.Euler(randMovementDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextDirection, Random.Range(0.6f, 2.3f));
        selectDirection = false;
    }

    private IEnumerator EnemyCooldown()
    {
        enemyCooldownAttack = true;
        yield return new WaitForSeconds(enemyCooldown);
        enemyCooldownAttack = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch(actualState)
        {
            case(EnemyState.Idle):
                IdleEnemy();
                break;

            case(EnemyState.Follow):
                EnemyFollow();
                break;
            
            case(EnemyState.Attack):
                EnemyAttack();
                break;

            case(EnemyState.Death):
                EnemyDeath();
                break;
        }

        if(!notInRoom){
            if(PlayerInEnemyRange(enemyRange) && actualState != EnemyState.Death)
            {
                actualState = EnemyState.Follow;
            }
            else if (!PlayerInEnemyRange(enemyRange) && actualState != EnemyState.Death)
            {
                actualState = EnemyState.Idle;
            }

            if(Vector3.Distance(transform.position, player.transform.position) <= enemyAttackRange)
            {
                actualState = EnemyState.Attack;
            }
        } else {
            actualState = EnemyState.Idle;
        }
    }

    void IdleEnemy()
    {
        // if(!selectDirection)
        // {
        //     StartCoroutine(SelectDirection());
        // }
        // transform.position += transform.right * enemySpeed * Time.deltaTime;

        if(PlayerInEnemyRange(enemyRange))
        {
            actualState = EnemyState.Follow;
        }
    }

    void EnemyFollow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemySpeed * Time.deltaTime);
    }

    void EnemyAttack()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemySpeed * Time.deltaTime);
        if(!enemyCooldownAttack)
        {
            switch (enemyType)
            {
                case EnemyType.BasicDron:
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<BulletController>().ShootToPlayer(player.transform, enemyDamage, enemyShootSpeed);
                    bullet.GetComponent<BulletController>().enemyShoot = true;
                    StartCoroutine(EnemyCooldown());
                    break;

                case EnemyType.MeleeDron:
                    GameManager.PlayerDamage(enemyDamage);
                    StartCoroutine(EnemyCooldown());
                    break;
            }
        }  
    }

    void EnemyDeath()
    {
        DungeonRoomController.controllerInstance.StartCoroutine(DungeonRoomController.controllerInstance.DungeonRoomCoroutine());
        Destroy(gameObject);
    }

    public void KillEnemy()
    {
        EnemyDeath();
    }

}
