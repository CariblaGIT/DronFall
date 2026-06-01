using UnityEngine;

// Controla una puerta de una sala: teletransporta al jugador a la sala contigua y puede bloquearse mientras hay enemigos vivos
public class DungeonDoor : MonoBehaviour
{
    // Enum con los posibles tipos de puerta que puede haber en una sala
    public enum DoorDirection{
        top, left, bottom, right
    }

    public DoorDirection doorDirection;
    // GameObject usado para bloquear cada puerta (puerta cerrada) hasta que se maten todos los enemigos
    public GameObject doorColl;
    private GameObject player;
    private float off = 4f;

    // Compartido entre todas las puertas para que un teletransporte no vuelva a disparar inmediatamente la puerta de llegada
    private static float lastTeleportTime = -1f;
    private const float teleportCooldown = 0.3f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Este trigger comprueba en qué puerta entra el jugador para moverlo en la dirección correspondiente aplicando el offset
    void OnTriggerEnter2D(Collider2D coll){
        if(coll.CompareTag("Player") && Time.time > lastTeleportTime + teleportCooldown){
            lastTeleportTime = Time.time;
            switch(doorDirection){
                case DoorDirection.top:
                    player.transform.position = new Vector2(transform.position.x, transform.position.y + off);
                    break;
                case DoorDirection.left:
                    player.transform.position = new Vector2(transform.position.x - off, transform.position.y);
                    break;
                case DoorDirection.right:
                    player.transform.position = new Vector2(transform.position.x + off, transform.position.y);
                    break;
                case DoorDirection.bottom:
                    player.transform.position = new Vector2(transform.position.x, transform.position.y - off);
                    break;
            }
        }
    }
}
