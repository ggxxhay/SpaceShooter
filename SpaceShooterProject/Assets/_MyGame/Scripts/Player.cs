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
    public float xMax = 5.2f, xMin = -5.2f, zMax = 5f, zMin = -9.15f;
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

    // Variables used to move with mouse drag
    private Vector3 screenPoint;
    private Vector3 offset;

    // Define if user is touching screen, use in moving player
    private bool isTouched;

    // Use this for initialization
    private void Start()
    {
        isTouched = false;

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
                PlayerBullet pBullet = bullet.GetComponent<PlayerBullet>();

                // If the bullet is not active, shoot it.
                if (pBullet.isActive == false)
                {
                    pBullet.transform.position = transform.position;
                    pBullet.isActive = true;
                    nextFire = Time.time + shotDelay;
                    return;
                }
            }

            GameObject newBullet = Instantiate(bullets[bulletType], transform.position, transform.rotation);
            poolingBullets.Add(newBullet);

            nextFire = Time.time + shotDelay;
        }
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

    private void MoveByTouch()
    {
        if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            if (!isTouched)
            {
                Vector3 startPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));

                offset = startPos - transform.position;

                isTouched = true;
            }
            //offset.y = 0;

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

    private void OnTriggerEnter(Collider other)
    {
        string layer = LayerMask.LayerToName(other.gameObject.layer);

        // Destroy player and enemy bullet when collision
        if (layer == "EnemyBullet")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
        // Destroy gift, increase player bullet's damage when player collision with gift
        else if (layer == "Gift")
        {
            if (bulletType >= bullets.Length - 1)
            {
                bullets[bulletType].GetComponent<PlayerBullet>().damage++;
            }
            else
            {
                bulletType++;
            }
            Destroy(other.gameObject);
        }
    }

    // Save score, high scores and notify game over to user
    private void OnDestroy()
    {
        // Save score and high scores before leave the scene
        transform.GetComponent<HighScore>().Save();

        FindObjectOfType<GameController>().GameOver();

        //foreach (var t in gameOverTexts)
        //{
        //    t.SetActive(true);
        //}

        //// Change position and font properties of score UI text
        //RectTransform scoreUIRectTransform = scoreUI.GetComponent<RectTransform>();
        //scoreUIRectTransform.anchoredPosition = new Vector2(0, 0);
        //scoreUIRectTransform.sizeDelta = new Vector2(400, 120);
        //scoreUI.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        //scoreUI.GetComponent<Text>().fontSize = 70;
    }

    public void AchievementNotice()
    {

    }

    // Return to Main menu
    public void ToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}