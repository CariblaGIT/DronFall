using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public string itemDescription;
    public Sprite itemImage;
}

public class ItemsController : MonoBehaviour
{
    public Item item;
    public float increaseSpeed;
    public int addHealth;
    public float increaseAttackSpeed;
    public float increaseAttackRange;
    public float hugeBulletShoot;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            // Do between all actions:
            GameManager.PlayerHeal(addHealth);
            GameManager.PlayerIncreaseSpeed(increaseSpeed);
            GameManager.PlayerIncreaseRange(increaseAttackRange);
            GameManager.PlayerIncreaseAttackSpeed(increaseAttackSpeed);
            GameManager.PlayerIncreaseShoot(hugeBulletShoot);
            Destroy(gameObject);
        }
    }
}
