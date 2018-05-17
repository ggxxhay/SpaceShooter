//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Types of hazards in a wave
    public GameObject[] hazards;

    // Object pooling
    public static List<GameObject> poolingEnemyBullets;
    public static List<GameObject> poolingEnemies;
    public static List<GameObject> poolingGift;

    public GameObject boss;
    private GameObject bossInstance;

    public List<GameObject> hazardsAlive;

    public Boundary boundary;

    // Number of hazard objects in a wave
    public int hazardsInWave;

    // Hazrad alive
    //public int hazardsAlive;

    public int currentWave;

    // Time to spawn other hazard in a wave
    public float spawnWaitTime = 3f;

    // Check status of Coroutine
    public bool isRunning;

    // Delay time for waves
    public WaitForSeconds WaveDelay = new WaitForSeconds(1f);

    // Texts in game scene
    private GameObject[] GameOverTexts;
    private GameObject ScoreUI;

    // Use this for initialization
    IEnumerator Start()
    {
        InitializeUI();
        poolingEnemyBullets = new List<GameObject>();
        poolingEnemies = new List<GameObject>();
        poolingGift = new List<GameObject>();

        // Hide game over texts
        foreach (var t in GameOverTexts)
        {
            t.SetActive(false);
        }

        while (true)
        {
            currentWave++;

            // Each 5 waves, a boss wave appear
            if (currentWave % 5 == 0)
            {
                SpawnBoss();
            }
            else
            {
                hazardsInWave = currentWave * 5;

                //hazardsAlive = hazardsInWave;

                // Set health and point for hazards
                foreach (var gameObject in hazards)
                {
                    SetHealthAndPoint(gameObject);
                }

                //StartCoroutine(Spawn());
                while (hazardsInWave > 0)
                {
                    SpawnHazard();

                    //hazardsAlive.Add(go);

                    //if(hazardsAlive[0].gameObject == null)
                    //{
                    //    print("Object destroyed: " + go.ToString());
                    //}

                    hazardsInWave--;
                    yield return new WaitForSeconds(spawnWaitTime);
                }
            }

            // Reduce hazards spawn delay each wave
            if (spawnWaitTime > 1)
            {
                spawnWaitTime -= 0.5f;
            }

            yield return WaveDelay;
        }
    }

    /// <summary>
    /// Spawn the enemy
    /// </summary>
    private void SpawnHazard()
    {
        // Limit spawn position of hazards in boundary
        Vector3 spawnPosition = new Vector3(Random.Range(boundary.xMin, boundary.xMax), 0, 11);

        // Find inactive enemy and spawn it
        foreach (var enemy in poolingEnemies)
        {
            if (enemy.GetComponent<ObjectPooling>().isActive == false)
            {
                enemy.transform.position = spawnPosition;
                enemy.GetComponent<ObjectPooling>().isActive = true;
                if(enemy.tag == "Enemy")
                {
                    enemy.GetComponent<Enemy>().canShoot = true;
                }
                SetHealthAndPoint(enemy);
                return;
            }
        }

        // Create new enemy
        poolingEnemies.Add(Instantiate(hazards[Random.Range(0, hazards.Length)],
            spawnPosition, new Quaternion(180, 0, 0, 0)));
    }

    /// <summary>
    /// Set health and point for hazard object
    /// </summary>
    /// <param name="gameObject"></param>
    private void SetHealthAndPoint(GameObject gameObject)
    {
        Hazards hazard = gameObject.GetComponent<Hazards>();
        hazard.hp = currentWave * 2;
        hazard.point = currentWave * 5;
    }


    /// <summary>
    /// Spawn Boss
    /// </summary>
    private void SpawnBoss()
    {
        Hazards h = bossInstance.GetComponent<Hazards>();
        h.hp = currentWave * 5;
        h.point = currentWave * 10;
        if (bossInstance.GetComponent<ObjectPooling>().isActive == false)
        {
            bossInstance.transform.position = new Vector3(0, 0, 11);
            bossInstance.GetComponent<Enemy>().canShoot = true;
            return;
        }
        Instantiate(boss, new Vector3(0, 0, 11), new Quaternion(180, 0, 0, 0));
        //bossInstance.transform.position = new Vector3(0, 0, 11);
    }

    // Setup scene's UI texts
    private void InitializeUI()
    {
        ScoreUI = GameObject.Find("Score");

        GameOverTexts = new GameObject[]
        {
            GameObject.Find("GameOver"),
            GameObject.Find("Back")
        };
    }

    public void RemovePoolingObject(GameObject go)
    {
        go.GetComponent<ObjectPooling>().isActive = false;

        string layerName = LayerMask.LayerToName(go.layer);

        if(layerName == "EnemyBullet" || layerName == "PlayerBullet")
        {
            go.transform.position = Boundary.InvisibleZoneBullet;
        }
        else
        {
            go.transform.position = Boundary.InvisibleZoneEnemy;
            if(layerName == "Boss" || layerName == "Enemy")
            {
                go.GetComponent<Enemy>().canShoot = false;
            }
        }
    }

    /// <summary>
    /// Report an achievement
    /// </summary>
    /// <param name="id">Achievement's ID</param>
    public void ReportAchievement(string id)
    {
        Social.ReportProgress(id, 100, success =>
        {
            if (success)
            {
                print("Success report: " + id);
                StartCoroutine(NotifyAchievement(id));
            }
            else
            {
                print("Report failed: " + id);
            }
        });
        PlayerPrefs.SetInt(id, 100);
    }

    /// <summary>
    /// Notify player about money problem.
    /// </summary>
    /// <returns></returns>
    IEnumerator NotifyAchievement(string achievementID)
    {
        int descriptionIndex = System.Convert.ToInt32(achievementID.Substring(achievementID.Length - 1)) - 1;

        GameObject noticeText = GameOverTexts[0];
        noticeText.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            // If the game is over, stop the coroutine.
            if (GameOverTexts[1].activeSelf)
            {
                GameOverTexts[0].GetComponent<Text>().text = "Game Over";
                yield break;
            }

            noticeText.GetComponent<Text>().text = "";
            yield return new WaitForSeconds(0.12f);
            noticeText.GetComponent<Text>().text = "Achievement Obtained: \n"
                + AchievementManager.AchievementDescription[descriptionIndex];
            yield return new WaitForSeconds(0.12f);
        }
        yield return new WaitForSeconds(2f);

        // If the game is over, stop the coroutine.
        if (GameOverTexts[1].activeSelf)
        {
            yield break;
        }
        noticeText.SetActive(false);
    }

    /// <summary>
    /// Display UI texts when the game is over
    /// </summary>
    public void GameOver()
    {
        GameOverTexts[0].GetComponent<Text>().text = "Game Over";
        foreach (var t in GameOverTexts)
        {
            t.SetActive(true);
        }

        // Change position and font properties of score UI text
        RectTransform scoreUIRectTransform = ScoreUI.GetComponent<RectTransform>();
        scoreUIRectTransform.anchoredPosition = new Vector2(0, 0);
        scoreUIRectTransform.sizeDelta = new Vector2(400, 120);
        ScoreUI.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        ScoreUI.GetComponent<Text>().fontSize = 70;
    }
}
