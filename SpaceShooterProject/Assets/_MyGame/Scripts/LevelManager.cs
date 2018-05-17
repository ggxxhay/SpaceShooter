﻿//using GooglePlayGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

[System.Serializable]
public class Leaderboard
{
    static public ILeaderboard leaderboard;
}

public class LevelManager : MonoBehaviour
{
    public GameObject[] otherMenus;

    // A temporary menu object
    private GameObject menuTemp;

    private GameObject mainMenu;
    private GameObject backButton;

    static private bool once = true;

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        //PlayerPrefs.DeleteKey(Keys.enemyKilledKey);

        Assets.SimpleAndroidNotifications.NotificationManager.CancelAll();

        mainMenu = GameObject.Find("Menu");
        backButton = GameObject.Find("Back");

        backButton.SetActive(false);

        foreach (var menu in otherMenus)
        {
            menu.SetActive(false);
        }

        Authenticate();
    }

    private void Update()
    {
        Escape();
    }

    /// <summary>
    /// Execution when escape button pressed
    /// </summary>
    public void Escape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mainMenu.activeSelf)
            {
                Quit();
            }
            else
            {
                ToMainMenu();
            }
        }
    }

    // Authenticate local user
    public void Authenticate()
    {
        Social.localUser.Authenticate(success =>
        {
            if (success)
            {
                Debug.Log("Authentication successful");
                string userInfo = "Username: " + Social.localUser.userName +
                    "\nUser ID: " + Social.localUser.id +
                    "\nIsUnderage: " + Social.localUser.underage;
                Debug.Log(userInfo);
            }
            else
                Debug.Log("Authentication failed");
        });
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Exit Application
    /// </summary>
    public void Quit()
    {
        Assets.SimpleAndroidNotifications.NotificationManager.
            Send(TimeSpan.FromSeconds(10), "I miss you", "Long time no see. Please come back!", Color.blue);
        Application.Quit();
    }

    // More game 
    public void MoreGame()
    {
        Application.OpenURL("https://docs.unity3d.com/ScriptReference/Application.OpenURL.html");
    }

    // Setting
    public void Setting()
    {
        //menuRect.anchoredPosition = new Vector2(-500, 0);
        //menu.transform.position = new Vector2(-200, 512);
        GameObject.Find("Setting").transform.position = new Vector3(0, 0, 0);
    }

    // Change menu when a menu is clicked
    public void ChangeMenu(string menuName)
    {
        mainMenu.SetActive(false);

        // Change to this menu
        foreach (GameObject otherMenu in otherMenus)
        {
            if (otherMenu.name == menuName)
            {
                menuTemp = otherMenu;
                otherMenu.SetActive(true);

                // Show achievement
                if (menuName == "AchievementMenu")
                {
                    GetComponent<AchievementManager>().LoadAchievement();
                }

                // Show highScores
                if(menuName == "HighScoreMenu")
                {
                    //GetComponent<LeaderboardManager>().ShowScore();

                    GetComponent<HighScore>().DisplayHighScores();
                }

                break;
            }
        }

        backButton.SetActive(true);
    }

    // Back to main menu
    public void ToMainMenu()
    {
        mainMenu.SetActive(true);

        // Save prices and skin color when out shop menu
        if (menuTemp.name == "ShopMenu")
        {
            Destroy(FindObjectOfType<ShopManager>().Player);
            FindObjectOfType<ShopManager>().SaveInfo();
        }

        menuTemp.SetActive(false);

        backButton.SetActive(false);
    }
}
