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

    public int currentWave = 0;

    // Time to spawn other hazard in a wave
    public float spawnWaitTime = 3f;

    // Check status of Coroutine
    public bool isRunning;

    // Delay time for waves
    public WaitForSeconds WaveDelay = new WaitForSeconds(3f);

    // Texts in game scene
    private GameObject[] gameOverTexts;
    private GameObject scoreUI;
    private GameObject waveInfo;
    public int waveInfoSpeed;

    // Use this for initialization
    IEnumerator Start()
    {
        InitializeUI();
        poolingEnemyBullets = new List<GameObject>();
        poolingEnemies = new List<GameObject>();
        poolingGift = new List<GameObject>();

        // Hide game over texts
        foreach (var t in gameOverTexts)
        {
            t.SetActive(false);
        }

        while (true)
        {
            // If the game is not over, continue spawning waves.
            if (!gameOverTexts[1].activeSelf)
            {
                currentWave++;

                // Show text inform that the wave is starting.
                StartCoroutine(Notify(waveInfo, "Wave " + currentWave));

                // Each 5 waves, a boss wave appear.
                if (currentWave % 5 == 0)
                {
                    SpawnBoss();

                    // Wait for wave to be completed.
                    while (bossInstance.GetComponent<ObjectPooling>().isActive)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
                else
                {
                    hazardsInWave = currentWave * 5;

                    // Set health and point for hazards.
                    foreach (var gameObject in hazards)
                    {
                        SetHealthAndPoint(gameObject, 2, 5);
                    }

                    //StartCoroutine(Spawn());
                    while (hazardsInWave > 0)
                    {
                        SpawnHazard();
                        hazardsInWave--;
                        yield return new WaitForSeconds(spawnWaitTime);
                    }

                    // Wait for all hazards in wave to be inactive to end the wave.
                    foreach (var hazard in poolingEnemies)
                    {
                        while (hazard.GetComponent<ObjectPooling>().isActive)
                        {
                            yield return new WaitForEndOfFrame();
                        }
                    }
                }

                // Reduce hazards spawn delay each wave.
                if (spawnWaitTime > 1)
                {
                    spawnWaitTime -= 0.5f;
                }

                // Show text inform that the wave is completed.
                StartCoroutine(Notify(waveInfo, "Wave Completed!"));

                yield return WaveDelay;
            }
            else
            {
                yield break;
            }
        }
    }

    /// <summary>
    /// Twinking text UI.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Notify(GameObject noticeText, string message)
    {
        noticeText.SetActive(true);
        isRunning = true;
        for (int i = 0; i < 5; i++)
        {
            noticeText.GetComponent<Text>().text = "";
            yield return new WaitForSeconds(0.15f);
            noticeText.GetComponent<Text>().text = message;
            yield return new WaitForSeconds(0.15f);
        }
        yield return new WaitForSeconds(0.5f);
        noticeText.SetActive(false);
    }

    private void EndWave()
    {

    }

    /// <summary>
    /// Setup scene's UI texts
    /// </summary>
    private void InitializeUI()
    {
        scoreUI = GameObject.Find("Score");
        waveInfo = GameObject.Find("WaveInfo");

        gameOverTexts = new GameObject[]
        {
            GameObject.Find("GameOver"),
            GameObject.Find("Back")
        };
    }

    /// <summary>
    /// Spawn the enemy
    /// </summary>
    private void SpawnHazard()
    {
        // Limit spawn position of hazards in boundary
        Vector3 spawnPosition = new Vector3(Random.Range(boundary.xMin, boundary.xMax), 0, 11);

        SpawnPoolingObject(spawnPosition, Quaternion.Euler(180, 0, 0),
            hazards[Random.Range(0, hazards.Length)], poolingEnemies);
    }

    /// <summary>
    /// Spawn method used for hazards, gift
    /// </summary>
    /// <param name="spawnPosition">Position to spawn</param>
    /// <param name="spawnRotation">Object's rotation</param>
    /// <param name="instance">Prefab to instantiate</param>
    /// <param name="poolingList">Pooling objects list</param>
    public void SpawnPoolingObject(Vector3 spawnPosition, Quaternion spawnRotation,
                                    GameObject instance, List<GameObject> poolingList)
    {
        print("Spawning");
        foreach (var poolingObject in poolingList)
        {
            // If the object is not active, then activate and spawn it.
            if (poolingObject.GetComponent<ObjectPooling>().isActive == false)
            {
                poolingObject.transform.position = spawnPosition;
                poolingObject.transform.rotation = spawnRotation;
                poolingObject.GetComponent<ObjectPooling>().isActive = true;

                if (poolingObject.tag == "Enemy" || poolingObject.tag == "Asteroid")
                {
                    SetHealthAndPoint(poolingObject, 2, 5);
                    if (poolingObject.tag == "Enemy")
                    {
                        poolingObject.GetComponent<Enemy>().canShoot = true;
                    }
                }

                return;
            }
        }

        // If there is no available object, create a new one
        poolingList.Add(Instantiate(instance, spawnPosition, spawnRotation));
        print("Spawning ------ Done");
    }

    /// <summary>
    /// Set health and point for hazard object
    /// </summary>
    /// <param name="gameObject">Hazard object</param>
    /// <param name="hpRate">Health rate</param>
    /// <param name="pointRate">Point rate</param>
    private void SetHealthAndPoint(GameObject gameObject, int hpRate, int pointRate)
    {
        Hazards hazard = gameObject.GetComponent<Hazards>();
        hazard.hp = currentWave * hpRate;
        hazard.point = currentWave * pointRate;
    }


    /// <summary>
    /// Spawn Boss
    /// </summary>
    private void SpawnBoss()
    {
        if (bossInstance == null)
        {
            SetHealthAndPoint(boss, 5, 10);
            bossInstance = Instantiate(boss, new Vector3(0, 0, 11), new Quaternion(180, 0, 0, 0));
        }
        else
        {
            SetHealthAndPoint(bossInstance, 5, 10);
            bossInstance.transform.position = new Vector3(0, 0, 11);
            bossInstance.GetComponent<ObjectPooling>().isActive = true;
            bossInstance.GetComponent<Enemy>().canShoot = true;
        }
    }

    /// <summary>
    /// Remove pooling object to invisible zone
    /// </summary>
    /// <param name="poolingObject">Game object to remove</param>
    public void RemovePoolingObject(GameObject poolingObject)
    {
        poolingObject.GetComponent<ObjectPooling>().isActive = false;

        string layerName = LayerMask.LayerToName(poolingObject.layer);

        if (layerName == "EnemyBullet" || layerName == "PlayerBullet")
        {
            poolingObject.transform.position = Boundary.InvisibleZoneBullet;
        }
        else
        {
            if (layerName == "Boss" || layerName == "Enemy" || layerName == "Asteroid")
            {
                poolingObject.GetComponent<Hazards>().hp = 1;
                if (layerName == "Boss" || layerName == "Enemy")
                {
                    poolingObject.GetComponent<Enemy>().canShoot = false;
                }
            }
            poolingObject.transform.position = Boundary.InvisibleZoneEnemy;
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

        GameObject noticeText = gameOverTexts[0];
        noticeText.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            // If the game is over, stop the coroutine.
            if (gameOverTexts[1].activeSelf)
            {
                gameOverTexts[0].GetComponent<Text>().text = "Game Over";
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
        if (gameOverTexts[1].activeSelf)
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
        gameOverTexts[0].GetComponent<Text>().text = "Game Over";
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

        GameObject.Find("BackButton").SetActive(false);
    }
}
