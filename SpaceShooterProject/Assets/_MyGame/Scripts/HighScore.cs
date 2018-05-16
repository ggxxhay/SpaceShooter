﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Keys
{
    // Key for total score which is used in Shop Menu
    static public string totalScoreKey = "total";

    // All time killed enemies (used in Achievement)
    static public string enemyKilledKey = "enemykilled";
}

public class HighScore : MonoBehaviour
{
    // Keys of top 5 highScore
    private string[] highScoresKeys;

    // Used to highScores data
    private int[] highScoresArray;

    // Texts to display
    public Text[] displayTexts;

    // Define if the highScoresArray is descending or not
    private bool isDescending;

    // Use this for initialization
    void Start()
    {
        highScoresKeys = new string[] { "h1", "h2", "h3", "h4", "h5" };
        highScoresArray = new int[5];
        GetHighScores();
        DisplayHighScores();
    }

    // Get highScores data
    private void GetHighScores()
    {
        for (int i = 0; i < highScoresKeys.Length; i++)
        {
            highScoresArray[i] = PlayerPrefs.GetInt(highScoresKeys[i]);
        }

        Array.Sort(highScoresArray);
        isDescending = false;
    }

    // Display high scores of players
    public void DisplayHighScores()
    {
        // Sort by descending
        if (!isDescending)
        {
            Array.Reverse(highScoresArray);
            isDescending = true;
        }

        // Show high scores to UI texts
        for (int i = 0; i < displayTexts.Length; i++)
        {
            displayTexts[i].text = "Top " + (i + 1) + ": " + highScoresArray[i].ToString();
        }
    }

    // Save player's total score and high scores
    public void Save()
    {
        int score = FindObjectOfType<Score>().GetScore();

        // Save total player score
        PlayerPrefs.SetInt(Keys.totalScoreKey, PlayerPrefs.GetInt(Keys.totalScoreKey) + score);

        // Sort by ascending
        if (isDescending)
        {
            Array.Sort(highScoresArray);
            isDescending = false;
        }

        print("Accessing score info!!!!!!!!");

        //UpdateHighScore(score);
        for (int i = 0; i < highScoresArray.Length; i++)
        {
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

    // Update highScore if it is smaller than current score
    private void UpdateHighScore(int score)
    {
        for (int i = 0; i < highScoresArray.Length; i++)
        {
            if (highScoresArray[i] <= score)
            {
                highScoresArray[i] = score;
                break;
            }
        }
    }
}
