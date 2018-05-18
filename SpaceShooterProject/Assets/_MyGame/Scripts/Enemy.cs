using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float shotDelay;
    public GameObject bullet;
    public bool canShoot = true;

    // Use this for initialization
    IEnumerator Start()
    {
        while (true)
        {
            if (canShoot)
            {
                if (transform.tag == "Boss")
                {
                    // i = 1 because we must pass through the enemy's engine (which index is 0)
                    for (int i = 1; i < transform.childCount; i++)
                    {
                        Transform shotPosition = transform.GetChild(i);
                        Shoot(shotPosition);
                    }
                }
                else
                {
                    Shoot(transform);
                }
            }
            yield return new WaitForSeconds(shotDelay);
        }
    }

    /// <summary>
    /// Shoot bullet
    /// </summary>
    /// <param name="shotPosition">Used to get initial position for the bullet</param>
    void Shoot(Transform shotPosition)
    {
        GetComponent<AudioSource>().Play();

        FindObjectOfType<GameController>().
            SpawnPoolingObject(shotPosition.position, shotPosition.rotation, bullet, GameController.poolingEnemyBullets);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
