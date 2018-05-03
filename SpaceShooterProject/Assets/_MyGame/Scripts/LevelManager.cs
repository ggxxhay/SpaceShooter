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
}
