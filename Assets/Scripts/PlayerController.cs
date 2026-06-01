using UnityEngine;

// Controla el movimiento y el disparo del jugador usando dos ejes independientes: movimiento y dirección de disparo
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{

    public float speed;
    Rigidbody2D body;
    Animator animator;
    public GameObject bulletObject;
    public float bulletSpeed;
    public float shootDelay;
    private float lastFireBullet;

    [SerializeField] private AudioClip moveSound;
    private AudioSource moveAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        moveAudioSource = GetComponent<AudioSource>();
        moveAudioSource.clip = moveSound;
        moveAudioSource.loop = true;
        moveAudioSource.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        speed = GameManager.MovementSpeed;
        bulletSpeed = GameManager.FireSpeed;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        // Eje de disparo independiente del eje de movimiento
        float shootHorizontal = Input.GetAxis("ShootHorizontal");
        float shootVertical = Input.GetAxis("ShootVertical");

        if((shootHorizontal != 0 || shootVertical != 0) && Time.time > lastFireBullet + shootDelay)
        {
            Shoot(shootHorizontal, shootVertical);
            lastFireBullet = Time.time;
        }

        body.velocity = new Vector3(horizontal * speed, vertical * speed, 0);

        UpdateMoveSound(horizontal != 0 || vertical != 0);
    }

    // Reproduce el sonido de desplazamiento en bucle mientras el jugador se mueve, y lo para en cuanto se queda quieto
    private void UpdateMoveSound(bool isMoving)
    {
        moveAudioSource.volume = SettingsManager.SfxVolume;

        if(isMoving && !moveAudioSource.isPlaying)
        {
            moveAudioSource.Play();
        } else if(!isMoving && moveAudioSource.isPlaying)
        {
            moveAudioSource.Stop();
        }
    }

    // Redondea el input de disparo a una de las 8 direcciones e instancia una bala con la velocidad y rotación correspondientes
    private void Shoot(float x, float y)
    {
        Vector2 direction = new Vector2(
            (x < 0) ? Mathf.Floor(x) : Mathf.Ceil(x),
            (y < 0) ? Mathf.Floor(y) : Mathf.Ceil(y)
        );
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject bullet = Instantiate(bulletObject, transform.position, Quaternion.Euler(0, 0, angle)) as GameObject;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }
}
