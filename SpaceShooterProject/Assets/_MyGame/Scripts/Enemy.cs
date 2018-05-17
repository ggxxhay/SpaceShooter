using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float shotDelay;
    public GameObject bullet;

    // Use this for initialization
    IEnumerator Start()
    {
        while (true)
        {
            if (transform.tag == "Boss")
            {
                // i=1 because we must pass through the enemy's engine (which index is 0)
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
            yield return new WaitForSeconds(shotDelay);
        }
    }

    // Shoot bullet
    void Shoot(Transform transform)
    {
        GetComponent<AudioSource>().Play();
        Instantiate(bullet, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
