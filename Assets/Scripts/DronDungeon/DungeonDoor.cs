using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDoor : MonoBehaviour
{
    // Enum with the possible types of doors that can be on a room
    public enum DoorDirection{
        top, left, bottom, right
    }

    public DoorDirection doorDirection;
    // This is a gameobject used to block each door (door closed) until you kill all enemies
    public GameObject doorColl;
    private GameObject player;
    private float off = 4f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // This trigger checks which door is entered to move the player to each direction applying the offset
    void OnTriggerEnter2D(Collider2D coll){
        if(coll.tag == "Player"){
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
