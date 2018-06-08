//using GooglePlayGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

[System.Serializable]
public class SkinColor
{
    static public Color32 playerSkinColor = new Color32(255, 255, 255, 255);
    static public string currentSkinIndexKey = "currentSkinIndexKey";

    static public Color32[] SkinColors 
        = new Color32[] {new Color32(255,255,255,255),
                        new Color32(62, 223, 255,255),
                        new Color32(255,237,0,255),
                        new Color32(248,106,106,255),
                        new Color32(8,122,255,255)
                        };
}

public class LevelManager : MonoBehaviour
{
    public GameObject[] otherMenus;

    // A temporary menu object
    private GameObject menuTemp;

    private GameObject mainMenu;
    private GameObject backButton;
    private GameObject callbackText;

    public int callbackReward;

    static private bool once = true;
    static private bool isSkinLoaded = false;

    //public static GameObject instance = null;

    private void Start()
    {
        //InitializeSingleton();

        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.DeleteKey(Keys.enemyKilledKey);
        callbackReward = 10;

        mainMenu = GameObject.Find("Menu");
        backButton = GameObject.Find("Back");
        callbackText = GameObject.Find("CallbackText");

        backButton.SetActive(false);

        // Load current selected color for player's ship.
        //FindObjectOfType<ShopManager>().LoadInitialColor();

        foreach (var menu in otherMenus)
        {
            menu.SetActive(false);
        }

        Authenticate();
        CheckNotifyCallBack();
    }

    private void Update()
    {
        Escape();
    }

    private void CheckNotifyCallBack()
    {
        if (FindObjectOfType<Test>().CheckCallBack() && once)
        {
            int currentScore = PlayerPrefs.GetInt(Keys.totalScoreKey);
            PlayerPrefs.SetInt(Keys.totalScoreKey, currentScore + callbackReward);
            StartCoroutine(Notify(callbackText, "Welcome back!\n" +
                                "You have received: " + callbackReward + " gold"));
            once = false;
        }
        else
        {
            callbackText.GetComponent<Text>().text = "";
        }
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

    /// <summary>
    /// Go to other scene, if it is MainGame Scene, load player skin color
    /// </summary>
    /// <param name="sceneName"></param>
    public void ChangeScene(string sceneName)
    {
        if (sceneName == "MainGame" && !isSkinLoaded)
        {
            LoadInitialColor();
        }
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Exit Application
    /// </summary>
    public void Quit()
    {
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
    /// Change from main menu to other menu when it is selected
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
                switch (menuName)
                {
                    case "AchievementMenu":
                        GetComponent<AchievementManager>().LoadAchievement();
                        break;
                    case "HighScoreMenu":
                        GetComponent<HighScore>().DisplayHighScores();
                        break;
                    case "ShopMenu":
                        isSkinLoaded = true;
                        break;

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

    public void LoadInitialColor()
    {
        // Set skin color info.
        int currentSkinIndex = PlayerPrefs.GetInt(SkinColor.currentSkinIndexKey);
        SkinColor.playerSkinColor = SkinColor.SkinColors[currentSkinIndex];
        print(currentSkinIndex.ToString());
    }
}
