using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float shotDelay;
    public float speed = 10;
    public float xMax = 5.2f, xMin = -5.2f, zMax = 5f, zMin = -9.15f;
    SpaceShip spaceShip;

    // Time to be enale to shoot the next shot
    public float nextFire;

    // Types of bullet
    public GameObject[] bullets;

    // Current bullet type index
    public int bulletType;

    // Use this for initialization
    private void Start()
    {
        spaceShip = GetComponent<SpaceShip>();
    }

    // Update is called once per frame
    void Update () {
        Move();

        // Shooting
        if (Input.GetButton("Fire1") && Time.time >= nextFire)
        {
            Instantiate(bullets[bulletType], transform.position, transform.rotation);
            nextFire = Time.time + shotDelay;
        }

        ChangeBullet();
    }

    // Change bullet type
    private void ChangeBullet()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            bulletType++;
            if (bulletType >= bullets.Length)
            {
                bulletType = 0;
            }
        }
    }

    // Move Player
    private void Move()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(moveHorizontal, 0, moveVertical).normalized;
        Vector3 pos = transform.position;
        pos += direction * speed * Time.deltaTime;
        pos = new Vector3(Mathf.Clamp(pos.x, xMin, xMax), 0, Mathf.Clamp(pos.z, zMin, zMax));
        transform.position = pos;
    }
}
