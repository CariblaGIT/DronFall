using System.Collections;
using UnityEngine;

// Estados posibles de la máquina de estados de un enemigo
public enum EnemyState
{
    Idle,
    Follow,
    Attack,
    Death
};

// Tipos de enemigo; KamikazeDron y CargoDron atacan igual que MeleeDron (daño de contacto), se diferencian
// ajustando enemySpeed/enemyHealth/enemyDamage en el Inspector de cada prefab
public enum EnemyType
{
    BasicDron,
    KamikazeDron,
    MeleeDron,
    CargoDron

}

// Máquina de estados simple de un enemigo (Idle -> Follow -> Attack -> Death) basada en la distancia al jugador
public class EnemyController : MonoBehaviour, IDamageable
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
    public int enemyHealth = 1;
    private bool enemyCooldownAttack = false;
    public bool notInRoom = false;
    public GameObject bulletPrefab;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip deathSound;

    // Comprueba si el jugador está dentro de un radio de distancia dado
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


    // Corrutina que mantiene al enemigo en cooldown entre ataques
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

    // Ejecuta el comportamiento del estado actual y decide si hay que cambiar de estado según la distancia al jugador
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

    // Estado de reposo: el enemigo no se mueve hasta detectar al jugador cerca
    void IdleEnemy()
    {
        if(PlayerInEnemyRange(enemyRange))
        {
            actualState = EnemyState.Follow;
        }
    }

    // Estado de persecución: el enemigo avanza hacia la posición actual del jugador
    void EnemyFollow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemySpeed * Time.deltaTime);
    }

    // Estado de ataque: sigue acercándose al jugador y, si no está en cooldown, ataca según su tipo (dispara o hace daño de contacto)
    void EnemyAttack()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemySpeed * Time.deltaTime);
        if(!enemyCooldownAttack)
        {
            AudioManager.PlaySfx(attackSound);

            switch (enemyType)
            {
                case EnemyType.BasicDron:
                    Vector3 shootDirection = player.transform.position - transform.position;
                    float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle)) as GameObject;
                    bullet.GetComponent<BulletController>().ShootToPlayer(player.transform, enemyDamage, enemyShootSpeed);
                    bullet.GetComponent<BulletController>().enemyShoot = true;
                    StartCoroutine(EnemyCooldown());
                    break;

                case EnemyType.MeleeDron:
                case EnemyType.KamikazeDron:
                case EnemyType.CargoDron:
                    GameManager.PlayerDamage(enemyDamage);
                    StartCoroutine(EnemyCooldown());
                    break;
            }
        }
    }

    // Resta vida al enemigo; lo mata al llegar a 0. Sustituye al antiguo "cualquier bala mata" para que
    // enemigos con mas enemyHealth (p.ej. CargoDron) aguanten varios golpes
    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        if(enemyHealth <= 0)
        {
            KillEnemy();
        }
    }

    // Estado de muerte: avisa al DungeonRoomController para que reevalúe el estado de la sala (puertas/enemigos restantes) y se destruye
    void EnemyDeath()
    {
        AudioManager.PlaySfx(deathSound);
        DungeonRoomController.controllerInstance.StartCoroutine(DungeonRoomController.controllerInstance.DungeonRoomCoroutine());
        Destroy(gameObject);
    }

    // Punto de entrada externo (p. ej. desde una bala del jugador) para matar a este enemigo
    public void KillEnemy()
    {
        EnemyDeath();
    }

}
