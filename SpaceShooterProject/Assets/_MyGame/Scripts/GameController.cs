//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Types of hazards in a wave
    public GameObject[] hazards;

    public GameObject boss;

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
                Hazards h = boss.transform.GetComponent<Hazards>();
                h.hp = currentWave * 5;
                h.point = currentWave * 10;
                Instantiate(boss, new Vector3(0, 0, 11), new Quaternion(180, 0, 0, 0));
            }
            else
            {
                hazardsInWave = currentWave * 5;

                //hazardsAlive = hazardsInWave;

                // Set health and point for hazards
                foreach (var gameObject in hazards)
                {
                    Hazards h = gameObject.transform.GetComponent<Hazards>();
                    h.hp = currentWave * 2;
                    h.point = currentWave * 5;
                }

                //StartCoroutine(Spawn());
                while (hazardsInWave > 0)
                {
                    // Limit spawn position of hazards in boundary
                    Vector3 spawnPosition = new Vector3(Random.Range(boundary.xMin, boundary.xMax), 0, 11);

                    GameObject go = Instantiate(hazards[Random.Range(0, hazards.Length)],
                        spawnPosition, new Quaternion(180, 0, 0, 0));

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
            if (spawnWaitTime > 0.5)
            {
                spawnWaitTime -= 0.5f;
            }

            yield return WaveDelay;
        }
    }

    private void InitializeUI()
    {
        ScoreUI = GameObject.Find("Score");

        GameOverTexts = new GameObject[]
        {
            GameObject.Find("GameOver"),
            GameObject.Find("Back")
        };
    }

    // Create hazards
    IEnumerator Spawn()
    {
        while (hazardsInWave > 0)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(boundary.xMin, boundary.xMax), 0, 11);
            Instantiate(hazards[Random.Range(0, hazards.Length)], spawnPosition, new Quaternion(180, 0, 0, 0));
            hazardsInWave--;
            yield return new WaitForSeconds(spawnWaitTime);
        }
    }

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
