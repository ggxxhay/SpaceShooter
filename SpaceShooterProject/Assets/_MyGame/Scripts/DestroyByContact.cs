using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class DestroyByContact : MonoBehaviour
{

    public GameObject explosion;
    public GameObject playerExplosion;
    public GameObject gift;

    public int enemyKilled;

    Hazards hazards;

    private void Start()
    {
        hazards = GetComponent<Hazards>();

        enemyKilled = PlayerPrefs.GetInt(Keys.enemyKilledKey);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DestroyArea")
        {
            //hazards.ReduceHazardsAlive();
            return;
        }

        if (other.tag == "Player")
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);
            return;
        }

        // If collider is player bullet, then reduce the hazard's health and move the bullet to invisible zone
        if (other.tag == "PlayerBullet")
        {
            Transform playerBulletTransform = other.transform;

            Bullet playerBullet = playerBulletTransform.GetComponent<Bullet>();

            // Change player bullet's position to invisible zone
            playerBulletTransform.position = Boundary.InvisibleZoneBullet;

            // Change pooling status to inactive
            other.GetComponent<ObjectPooling>().isActive = false;

            hazards.hp -= playerBullet.damage;
        }

        EnemyDead();
    }

    /// <summary>
    /// Instantiate gift object
    /// </summary>
    private void CreateGift()
    {
        // Create gift for player
        if (gameObject.tag == "Boss" || Random.Range(1, 5) == 1)
        {
            FindObjectOfType<GameController>().SpawnPoolingObject(transform.position, Quaternion.Euler(90, 0, 0), 
                                                                 gift, GameController.poolingGift);
        }
    }

    /// <summary>
    /// Execution when enemy is dead
    /// </summary>
    private void EnemyDead()
    {
        // Có thể tại máu dưới 0 mà khi ra safe zone, gift vẫn được tạo thêm 1 lần.
        if (hazards.hp <= 0 && FindObjectOfType<Player>() != null)
        {
            // Add score
            FindObjectOfType<Score>().AddPoint(gameObject.GetComponent<Hazards>().point);

            // Create the gift if the dead enemy is boss, or random as rate 1/5
            if (gameObject.tag == "Boss" || Random.Range(1, 5) == 1)
            {
                FindObjectOfType<GameController>().SpawnPoolingObject(transform.position, Quaternion.Euler(90, 0, 0),
                                                                     gift, GameController.poolingGift);
            }

            // Create Explosion
            Instantiate(explosion, transform.position, transform.rotation);

            FindObjectOfType<GameController>().RemovePoolingObject(gameObject);

            // Count on enemy killed
            enemyKilled++;
            PlayerPrefs.SetInt(Keys.enemyKilledKey, enemyKilled);

            if (enemyKilled == 1)
            {
                FindObjectOfType<GameController>().ReportAchievement("Achievement01");
                return;
            }
            if (enemyKilled == 10)
            {
                FindObjectOfType<GameController>().ReportAchievement("Achievement02");
                return;
            }
            if (enemyKilled == 100)
            {
                FindObjectOfType<GameController>().ReportAchievement("Achievement03");
                return;
            }
        }
    }
}
