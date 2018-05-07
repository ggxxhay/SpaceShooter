using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	// Change Scene
    public void ChangeLevel(string sceneName)
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
        RectTransform menuRect = GameObject.Find("Menu").GetComponent<RectTransform>();
        menuRect.anchoredPosition = new Vector2(-500, 0);
        //menu.transform.position = new Vector2(-200, 512);
        GameObject.Find("Slider").transform.position = new Vector3(0, 0, 0);
    }
}
