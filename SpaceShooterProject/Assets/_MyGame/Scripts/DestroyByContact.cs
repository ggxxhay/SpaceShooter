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
        }

        // If collider is player bullet, then reduce the hazard's health
        if (LayerMask.LayerToName(other.gameObject.layer) == "PlayerBullet")
        {
            Transform playerBulletTransform = other.transform;

            PlayerBullet playerBullet = playerBulletTransform.GetComponent<PlayerBullet>();

            hazards.hp -= playerBullet.damage;
        }

        Destroy(other.gameObject);
        //Destroy(playerExplosion);

        EnemyDead();
        
    }

    // Enemy health reduce to 0
    private void EnemyDead()
    {
        if (hazards.hp <= 0 && FindObjectOfType<Player>() != null)
        {
            // Add score
            FindObjectOfType<Score>().AddPoint(gameObject.GetComponent<Hazards>().point);

            // Create gift for player
            if (gameObject.tag == "Boss" || Random.Range(1, 5) == 1)
            {
                Instantiate(gift, transform.position, Quaternion.Euler(90, 0, 0));
            }

            // Create Explosion
            Instantiate(explosion, transform.position, transform.rotation);

            Destroy(gameObject);

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
