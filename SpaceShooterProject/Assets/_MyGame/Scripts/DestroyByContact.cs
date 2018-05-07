using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{

    public GameObject explosion;
    public GameObject playerExplosion;
    public GameObject gift;

    Hazards hazards;

    private void Start()
    {
        hazards = GetComponent<Hazards>();
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

        if (hazards.hp <= 0)
        {
            if (gameObject.tag == "Boss" || Random.Range(1, 5) == 1)
            {
                Instantiate(gift, transform.position, Quaternion.Euler(90, 0, 0));
            }
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
            //Destroy(explosion);
        }
    }
}
