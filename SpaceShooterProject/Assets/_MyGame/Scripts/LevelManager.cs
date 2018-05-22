//using GooglePlayGames;
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

    //static private bool once = true;

    //public static GameObject instance = null;

    private void Start()
    {
        //InitializeSingleton();

        //PlayerPrefs.DeleteAll();
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

    //public void InitializeSingleton()
    //{
    //    if (instance == null)
    //    {
    //        instance = this.gameObject;
    //        mainMenu = GameObject.Find("Menu");
    //        backButton = GameObject.Find("Back");
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //        //Destroy(mainMenu.gameObject);
    //        //Destroy(backButton.gameObject);
    //        //foreach (var menu in otherMenus)
    //        //{
    //        //    Destroy(menu.gameObject);
    //        //}
    //    }

    //    DontDestroyOnLoad(instance);
    //    DontDestroyOnLoad(mainMenu.gameObject);
    //    DontDestroyOnLoad(backButton.gameObject);
    //    foreach (var menu in otherMenus)
    //    {
    //        DontDestroyOnLoad(menu.gameObject);
    //    }
    //}

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

    /// <summary>
    /// Change menu when a menu is clicked
    /// </summary>
    /// <param name="menuName"></param>
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

    /// <summary>
    /// Back to main menu
    /// </summary>
    public void ToMainMenu()
    {
        mainMenu.SetActive(true);

        // Save prices and skin color when out shop menu
        if (menuTemp.name == "ShopMenu")
        {
            //Destroy(FindObjectOfType<ShopManager>().Player.gameObject);
            FindObjectOfType<ShopManager>().Player.SetActive(false);
            FindObjectOfType<ShopManager>().SaveInfo();
        }

        menuTemp.SetActive(false);

        backButton.SetActive(false);
    }

    /// <summary>
    /// Twinking text UI.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Notify(GameObject noticeText, string message)
    {
        noticeText.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            noticeText.GetComponent<Text>().text = "";
            yield return new WaitForSeconds(0.15f);
            noticeText.GetComponent<Text>().text = message;
            yield return new WaitForSeconds(0.15f);
        }
    }
}
