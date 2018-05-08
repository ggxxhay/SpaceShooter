using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour
{

    // Keys of top 5 highScore
    private string[] highScoresKeys;

    // Total score to use in shop
    private string totalScoreKey;

    // Used to highScores data
    private int[] highScoresArray;

    // Use this for initialization
    void Start()
    {
        highScoresKeys = new string[] { "h1", "h2", "h3", "h4", "h5" };
        totalScoreKey = "total";
        highScoresArray = new int[5];
        GetHighScores();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Get highScores data
    private void GetHighScores()
    {
        for (int i = 0; i < highScoresKeys.Length - 1; i++)
        {
            if (PlayerPrefs.HasKey(highScoresKeys[i]))
            {
                highScoresArray[i] = PlayerPrefs.GetInt(highScoresKeys[i]);
            }
            else
            {
                highScoresArray[i] = 0;
            }
        }

        // Sort array descending
        Array.Sort(highScoresArray);
        Array.Reverse(highScoresArray);
    }


    public void Save()
    {
        int score = FindObjectOfType<Score>().GetScore();

        // Save total player score
        PlayerPrefs.SetInt(totalScoreKey, PlayerPrefs.GetInt(totalScoreKey) + score);

        // Update highScores
        for (int i = 0; i < highScoresArray.Length; i++)
        {
            if (highScoresArray[i] < score)
            {
                highScoresArray[i] = score;
                break;
            }
        }

        // Save highScores
        for (int i = 0; i < highScoresKeys.Length; i++)
        {
            PlayerPrefs.SetInt(highScoresKeys[i], highScoresArray[i]);
            PlayerPrefs.Save();
        }
    }
}
