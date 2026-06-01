using UnityEngine;

// Datos descriptivos de un ítem (nombre, descripción y sprite), no es un ScriptableObject
[System.Serializable]
public class Item
{
    public string itemName;
    public string itemDescription;
    public Sprite itemImage;
}

// Ítem recogible que mejora las estadísticas del jugador al tocarlo
public class ItemsController : MonoBehaviour
{
    public Item item;
    public float increaseSpeed;
    public int addHealth;
    public float increaseAttackSpeed;
    public float increaseAttackRange;
    public float hugeBulletShoot;
    [SerializeField] private AudioClip pickupSound;

    // Asigna el sprite del ítem y regenera su collider (como trigger) ajustado a los bounds reales de ese sprite.
    // Se usa BoxCollider2D en vez de PolygonCollider2D porque el autotrazado de este último depende de los márgenes
    // transparentes del PNG y puede generar un collider mucho más grande de lo que se ve dibujado
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if(item.itemImage != null)
        {
            spriteRenderer.sprite = item.itemImage;
        }

        Destroy(GetComponent<PolygonCollider2D>());
        Destroy(GetComponent<BoxCollider2D>());

        BoxCollider2D itemCollider = gameObject.AddComponent<BoxCollider2D>();
        itemCollider.isTrigger = true;
        itemCollider.size = spriteRenderer.sprite.bounds.size;
    }

    // Al tocar al jugador, aplica todas las mejoras configuradas en este ítem y se destruye
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            GameManager.PlayerHeal(addHealth);
            GameManager.PlayerIncreaseSpeed(increaseSpeed);
            GameManager.PlayerIncreaseRange(increaseAttackRange);
            GameManager.PlayerIncreaseAttackSpeed(increaseAttackSpeed);
            GameManager.PlayerIncreaseShoot(hugeBulletShoot);
            AudioManager.PlaySfx(pickupSound);
            Destroy(gameObject);
        }
    }
}
