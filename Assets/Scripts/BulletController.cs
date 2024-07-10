using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float lifeTime;
    public bool enemyShoot = false;
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
            transform.localScale = new Vector2(GameManager.FireSize, GameManager.FireSize);
        } 
    }

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

    IEnumerator BulletDeath()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    public void ShootToPlayer(Transform playerTransform, int damage, float shootSpeed)
    {
        playerPosition = playerTransform.position;
        damageDealt = damage;
        bulletSpeed = shootSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Enemy" && !enemyShoot)
        {
            other.gameObject.GetComponent<EnemyController>().KillEnemy();
            Destroy(gameObject);
        }

        if(other.tag == "Player" && enemyShoot)
        {
            GameManager.PlayerDamage(damageDealt);
            Destroy(gameObject);
        }
    }
}
