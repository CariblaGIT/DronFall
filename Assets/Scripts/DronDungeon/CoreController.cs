using UnityEngine;

// Objetivo final del ultimo nivel: el jugador debe destruirlo a tiros para ganar la partida. Necesita el tag
// "Enemy" (puesto a mano en el Inspector) para que BulletController.OnTriggerEnter2D lo detecte como objetivo
// de las balas del jugador, igual que a un EnemyController, a traves de la interfaz IDamageable.
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class CoreController : MonoBehaviour, IDamageable
{
    [SerializeField] private int coreHealth = 10;
    [SerializeField] private AudioClip destroySound;

    public void TakeDamage(int damage)
    {
        coreHealth -= damage;
        if(coreHealth <= 0)
        {
            AudioManager.PlaySfx(destroySound);
            GameManager.TriggerVictory();
            Destroy(gameObject);
        }
    }

    // Ajusta el collider de trigger al tamano del sprite al añadir el componente en el Editor
    private void Reset()
    {
        BoxCollider2D coll = GetComponent<BoxCollider2D>();
        coll.isTrigger = true;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer.sprite != null)
        {
            coll.size = spriteRenderer.sprite.bounds.size;
        }
    }
}
