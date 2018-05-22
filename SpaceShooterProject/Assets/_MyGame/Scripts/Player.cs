using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

[System.Serializable]
public class Boundary
{
    public float xMax = 5.7f, xMin = -5.7f, zMax = 5f, zMin = -9.15f;

    public static Vector3 InvisibleZoneBullet = new Vector3(15, 0, 0);

    public static Vector3 InvisibleZoneEnemy = new Vector3(-15, 0, 15);
}

public class Player : MonoBehaviour
{
    public float shotDelay;
    public float speed = 10;
    public Boundary boundary;
    public GameObject explosion;

    // Tilt when playerShip rotating
    public float tilt;

    // Time to be enale to shoot the next shot
    public float nextFire;

    // Types of bullet
    public GameObject[] bullets;

    // Bullets for pooling
    private List<GameObject> poolingBullets;

    // Current bullet type index
    public int bulletType;
    private bool isBulletTypeUpdated;

    // Variables used to move with mouse drag
    private Vector3 screenPoint;
    private Vector3 offset;

    // Define if user is touching screen, used in moving player
    private bool isTouched;

    // Use this for initialization
    private void Start()
    {
        isTouched = false;
        isBulletTypeUpdated = true;

        poolingBullets = new List<GameObject>();

        // Set skin for player
        gameObject.GetComponent<Renderer>().material.color = SkinColor.playerSkinColor;
    }

    // Update is called once per frame
    void Update()
    {
        MoveByTouch();
        Move();
        if (Input.touchCount <= 0)
        {
            isTouched = false;
            //offset = new Vector3(0, 0, 0);
        }
        //OnMouseDown();
        //OnMouseDrag();

        Shoot();

        ChangeBullet();
    }

    /// <summary>
    /// Shooting bullets
    /// </summary>
    private void Shoot()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFire)
        {
            GetComponent<AudioSource>().Play();

            foreach (var bullet in poolingBullets)
            {
                // If the bullet is not active, shoot it.
                if (bullet.GetComponent<ObjectPooling>().isActive == false)
                {
                    bullet.transform.position = transform.position;
                    bullet.GetComponent<ObjectPooling>().isActive = true;
                    nextFire = Time.time + shotDelay;
                    return;
                }
            }

            // Cannot call if-else because if statement is in the foreach loop
            GameObject newBullet = Instantiate(bullets[bulletType], transform.position, Quaternion.Euler(0, 0, 0));
            poolingBullets.Add(newBullet);

            nextFire = Time.time + shotDelay;
        }
    }

    /// <summary>
    /// Change bullet type (used for testing)
    /// </summary>
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

    /// <summary>
    /// Move Player with keyboard
    /// </summary>
    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(moveHorizontal, 0, moveVertical);
        GetComponent<Rigidbody>().velocity = direction * speed;
        Vector3 pos = transform.position;

        // Limit the position of Player
        GetComponent<Rigidbody>().position = new Vector3
            (Mathf.Clamp(pos.x, boundary.xMin, boundary.xMax),
            0,
            Mathf.Clamp(pos.z, boundary.zMin, boundary.zMax));

        // Rotate Player while moving left-right
        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0f, 0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }

    /// <summary>
    /// Move Player with screen touching
    /// </summary>
    private void MoveByTouch()
    {
        if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            if (!isTouched)
            {
                // The original position that player touch the screen
                Vector3 startPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));

                offset = startPos - transform.position;

                isTouched = true;
            }

            // If touch is holding or moving, then move player
            if (touch.phase == TouchPhase.Moved)
            {
                // Get touch position and convert to world point
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));

                touchPos -= offset;

                // Clamp moved position
                Vector3 clampedPos = new Vector3(
                        Mathf.Clamp(touchPos.x, boundary.xMin, boundary.xMax),
                        0,
                        Mathf.Clamp(touchPos.z, boundary.zMin, boundary.zMax)
                    );

                // Let player move smoothly.
                transform.position = Vector3.Lerp(transform.position, clampedPos, speed);
            }
        }
    }

    void OnMouseDown()
    {
        print("Mouse down");
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, screenPoint.y, Input.mousePosition.z));
    }

    void OnMouseDrag()
    {
        print("Mouse Drag");
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, screenPoint.y, Input.mousePosition.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
        //transform.position = cursorPosition;
        transform.position = new Vector3(cursorPosition.x, 0, cursorPosition.z);
    }

    /// <summary>
    /// Execution when player collide with enemy bullet and gift
    /// </summary>
    /// <param name="other">The other collider: enemy bullet or gift</param>
    private void OnTriggerEnter(Collider other)
    {
        string layer = LayerMask.LayerToName(other.gameObject.layer);

        // Destroy player and enemy bullet when collision
        if (layer == "EnemyBullet")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
            FindObjectOfType<GameController>().RemovePoolingObject(other.gameObject);
            return;
        }

        if (layer == "Gift")
        {
            ChangeBulletType();
            FindObjectOfType<GameController>().RemovePoolingObject(other.gameObject);
            return;
        }

    }

    /// <summary>
    /// Change bullet type for bullets in pool
    /// </summary>
    private void ChangeBulletType()
    {
        if (bulletType < bullets.Length - 1)
        {
            bulletType++;
            isBulletTypeUpdated = false;
        }
        else
        {
            isBulletTypeUpdated = true;
        }
        UpdateBullet();
    }

    /// <summary>
    /// Update bullet type and damage for bullets in pool
    /// </summary>
    private void UpdateBullet()
    {
        for (int i = 0; i < poolingBullets.Count; i++)
        {
            if (isBulletTypeUpdated)
            {
                poolingBullets[i].GetComponent<Bullet>().damage++;
            }
            else
            {
                poolingBullets[i] = bullets[bulletType];
                //poolingBullets[i].GetComponent<ObjectPooling>().isActive = false;
            }
        }
    }

    /// <summary>
    /// Save score, high scores and notify game over to user
    /// </summary>
    private void OnDestroy()
    {
        transform.GetComponent<HighScore>().Save();

        FindObjectOfType<GameController>().GameOver();
    }

    /// <summary>
    /// Return to Main menu
    /// </summary>
    public void ToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}