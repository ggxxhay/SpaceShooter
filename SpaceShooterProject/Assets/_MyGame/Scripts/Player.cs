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
    public GameObject scoreUI;

    // Text to display when game over
    public GameObject[] gameOverTexts;

    // Tilt when playerShip rotating
    public float tilt;

    // Time to be enale to shoot the next shot
    public float nextFire;

    // Types of bullet
    public GameObject[] bullets;

    // Current bullet type index
    public int bulletType;

    // Use this for initialization
    private void Start()
    {
        // Hide game over texts
        foreach (var t in gameOverTexts)
        {
            t.SetActive(false);
        }

        // Set skin for player
        gameObject.GetComponent<Renderer>().material.color = SkinColor.playerSkinColor;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        // Shooting bullets
        if (Input.GetButton("Fire1") && Time.time >= nextFire)
        {
            GetComponent<AudioSource>().Play();
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

        foreach (var t in gameOverTexts)
        {
            t.SetActive(true);
        }

        // Change position and font properties of score UI text
        RectTransform scoreUIRectTransform = scoreUI.GetComponent<RectTransform>();
        scoreUIRectTransform.anchoredPosition = new Vector2(0, 0);
        scoreUIRectTransform.sizeDelta = new Vector2(400, 120);
        scoreUI.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        scoreUI.GetComponent<Text>().fontSize = 70;
    }

    // Return to Main menu
    public void ToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
