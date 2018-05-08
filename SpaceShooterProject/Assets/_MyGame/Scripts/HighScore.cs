using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{

    // Keys of top 5 highScore
    private string[] highScoresKeys;

    // Total score to use in shop
    private string totalScoreKey;

    // Used to highScores data
    private int[] highScoresArray;

    // Texts to display
    public Text[] displayTexts;

    // Define if the highScoreArray is descending or not
    private bool isDescending;

    // Use this for initialization
    void Start()
    {
        highScoresKeys = new string[] { "h1", "h2", "h3", "h4", "h5" };
        totalScoreKey = "total";
        highScoresArray = new int[5];
        GetHighScores();
        DisplayHighScores();
    }

    // Display high scores of players
    private void DisplayHighScores()
    {
        // Sort by descending
        if (!isDescending)
        {
            Array.Reverse(highScoresArray);
            isDescending = true;
        }

        for (int i = 0; i < displayTexts.Length; i++)
        {
            displayTexts[i].text = "Top " + (i + 1) + ": " + highScoresArray[i].ToString();
        }
    }

    // Get highScores data
    private void GetHighScores()
    {
        for (int i = 0; i < highScoresKeys.Length; i++)
        {
            //if (PlayerPrefs.HasKey(highScoresKeys[i]))
            //{
            //    highScoresArray[i] = PlayerPrefs.GetInt(highScoresKeys[i]);
            //}
            //else
            //{
            //    highScoresArray[i] = 0;
            //}
            highScoresArray[i] = PlayerPrefs.GetInt(highScoresKeys[i]);
        }

        Array.Sort(highScoresArray);
        isDescending = false;
        foreach (int i in highScoresArray)
        {
            print(i);
        }
    }


    public void Save()
    {
        int score = FindObjectOfType<Score>().GetScore();

        // Save total player score
        PlayerPrefs.SetInt(totalScoreKey, PlayerPrefs.GetInt(totalScoreKey) + score);

        // Sort by ascending
        if (isDescending)
        {
            Array.Sort(highScoresArray);
            isDescending = false;
        }

        // Update highScores
        for (int i = 0; i < highScoresArray.Length; i++)
        {
            print(highScoresArray[i].ToString() + " --- " + score.ToString());
            if (highScoresArray[i] <= score)
            {
                highScoresArray[i] = score;
                break;
            }
        }

        // Save highScores
        for (int i = 0; i < highScoresKeys.Length; i++)
        {
            print(highScoresArray[i] + " - saved");
            PlayerPrefs.SetInt(highScoresKeys[i], highScoresArray[i]);
        }

        PlayerPrefs.Save();
    }
}
