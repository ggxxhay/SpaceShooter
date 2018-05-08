using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        //boundary = Get
        foreach (var t in gameOverTexts)
        {
            t.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
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
        GetComponent<Rigidbody>().velocity = direction * speed;
        Vector3 pos = transform.position;

        // Limit the position of Player
        GetComponent<Rigidbody>().position = new Vector3
            (Mathf.Clamp(pos.x, boundary.xMin, boundary.xMax),
            0,
            Mathf.Clamp(pos.z, boundary.zMin, boundary.zMax));

        // Rotate Player while moving
        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0f, 0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }

    // Destroy when hit
    private void OnTriggerEnter(Collider other)
    {
        string layer = LayerMask.LayerToName(other.gameObject.layer);
        if (layer == "EnemyBullet")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(other.gameObject);
            //Destroy(explosion);
        }
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

    private void OnDestroy()
    {
        transform.GetComponent<HighScore>().Save();
        foreach(var t in gameOverTexts)
        {
            t.SetActive(true);
        }
        RectTransform scoreUIRectTransform = scoreUI.GetComponent<RectTransform>();
        scoreUIRectTransform.anchoredPosition = new Vector2(0, 0);
        scoreUIRectTransform.sizeDelta = new Vector2(400, 110);
        scoreUI.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        scoreUI.GetComponent<Text>().fontSize = 60;
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
