using System.Collections;
using UnityEngine;

// Controla el comportamiento de una bala, tanto las del jugador como las de los enemigos (según el flag enemyShoot)
public class BulletController : MonoBehaviour
{

    public float lifeTime;
    public bool enemyShoot = false;
    [SerializeField] private AudioClip shootSound;
    private int damageDealt;
    private float bulletSpeed;

    private Vector2 lastPosition;
    private Vector2 actualPosition;
    private Vector2 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BulletDeath());
        if(!enemyShoot)
        {
            // Las balas del jugador escalan con la mejora de tamaño de disparo (GameManager.FireSize)
            transform.localScale *= GameManager.FireSize;
        }

        AudioManager.PlaySfx(shootSound);
    }

    // Las balas enemigas se mueven hacia la posición del jugador capturada al disparar; se autodestruyen si dejan de avanzar
    void Update()
    {
        if(enemyShoot)
        {
            actualPosition = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPosition, bulletSpeed * Time.deltaTime);
            if(actualPosition == lastPosition)
            {
                Destroy(gameObject);
            }
            lastPosition = actualPosition;
        }
    }

    // Destruye la bala pasado su tiempo de vida, la haya golpeado algo o no
    IEnumerator BulletDeath()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    // Configura una bala enemiga con el objetivo, el daño que hace y la velocidad a la que viaja
    public void ShootToPlayer(Transform playerTransform, int damage, float shootSpeed)
    {
        playerPosition = playerTransform.position;
        damageDealt = damage;
        bulletSpeed = shootSpeed;
    }

    // Al colisionar: las balas del jugador restan 1 de vida a cualquier IDamageable (enemigos, el nucleo final...),
    // las balas enemigas dañan al jugador; ambas se destruyen al impactar
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy" && !enemyShoot)
        {
            other.gameObject.GetComponent<IDamageable>()?.TakeDamage(1);
            Destroy(gameObject);
        }

        if(other.tag == "Player" && enemyShoot)
        {
            GameManager.PlayerDamage(damageDealt);
            Destroy(gameObject);
        }
    }
}
