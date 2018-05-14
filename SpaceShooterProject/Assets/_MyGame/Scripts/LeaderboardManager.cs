using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{

    public Text[] highScoresUI;

    ILeaderboard leaderboard;

    // Use this for initialization
    void Start()
    {
        CreateLeaderboard();
        //ShowScore();
    }

    void CreateLeaderboard()
    {
        leaderboard = Social.CreateLeaderboard();
        leaderboard.id = "Leaderboard01";
        Debug.Log("Create " + leaderboard.id + " successfully");
    }

    void ReportScore()
    {
        // Report a score to leaderboard
        Social.ReportScore(15, leaderboard.id, success =>
        {
            if (success)
            {
                Debug.Log("Success report!");
                Debug.Log("--- " + leaderboard.id + " ---");
            }
            else
            {
                Debug.Log("Error report!");
            }
        });
    }

    public void ShowScore()
    {
        // Load scores from leaderboard
        Social.LoadScores(leaderboard.id, scores =>
        {
            if (scores.Length > 0 && highScoresUI.Length > 0)
            {
                int i = 0;
                foreach (IScore score in scores)
                {
                    highScoresUI[i++].text = "Top " + (i).ToString() + ": " + score.value;
                    if (i >= highScoresUI.Length - 1)
                    {
                        break;
                    }
                }
            }
            else
            {
                print("There is no score yet");
            }
        });
    }
}
