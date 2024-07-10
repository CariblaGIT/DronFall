using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    Rigidbody2D body;
    public GameObject bulletObject;
    public float bulletSpeed;
    public float shootDelay;
    private float lastFireBullet;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = GameManager.MovementSpeed;
        bulletSpeed = GameManager.FireSpeed;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float shootHorizontal = Input.GetAxis("ShootHorizontal");
        float shootVertical = Input.GetAxis("ShootVertical");

        if((shootHorizontal != 0 || shootVertical != 0) && Time.time > lastFireBullet + shootDelay)
        {
            Shoot(shootHorizontal, shootVertical);
            lastFireBullet = Time.time;
        }

        body.velocity = new Vector3(horizontal * speed, vertical * speed, 0);
    }

    private void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(bulletObject, transform.position, Quaternion.identity) as GameObject;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,
            0
        );
    }
}
