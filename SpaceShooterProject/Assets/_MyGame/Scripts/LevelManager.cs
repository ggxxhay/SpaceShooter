using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    //private RectTransform menuRect;

    // 
    private GameObject menuTemp;

    private GameObject mainMenu;
    private GameObject backButton;

    private GameObject[] otherMenus;

    private void Start()
    {
        //menuRect = GameObject.Find("Menu").GetComponent<RectTransform>();
        mainMenu = GameObject.Find("Menu");
        backButton = GameObject.Find("Back");
        backButton.SetActive(false);

        otherMenus = new GameObject[] {
            GameObject.Find("SettingMenu"),
            GameObject.Find("HighScoreMenu")
        };

        foreach(var menu in otherMenus)
        {
            menu.SetActive(false);
        }

    }

    // Change Scene
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Exit Application
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

    // Change menu
    public void ChangeMenu(string menuName)
    {
        GameObject.Find("Menu").SetActive(false);

        // Change to this menu
        foreach(GameObject otherMenu in otherMenus)
        {
            if(otherMenu.name == menuName)
            {
                menuTemp = otherMenu;
                otherMenu.SetActive(true);
                break;
            }
        }

        backButton.SetActive(true);
    }

    // Back to main menu
    public void ToMainMenu()
    {
        mainMenu.SetActive(true);

        menuTemp.SetActive(false);

        backButton.SetActive(false);
    }
}
