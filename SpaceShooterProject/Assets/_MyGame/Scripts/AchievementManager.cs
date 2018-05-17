using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

public class AchievementManager : MonoBehaviour
{
    public static string[] AchievementDescription;

    public Text[] AchievementUI;

    // Use this for initialization
    void Start()
    {
        AchievementDescription = new string[]
        {
            "Kill first enemy",
            "Kill 10 enemies",
            "Kill 100 enemies"
        };
        CreateAchievement();
    }

    /// <summary>
    /// Create achievement instances
    /// </summary>
    private void CreateAchievement()
    {
        for (int i = 1; i <= 3; i++)
        {
            IAchievement achievement = Social.CreateAchievement();
            achievement.id = "Achievement0" + i.ToString();
            ReportAchievement(achievement.id);
        }
    }

    // Report achievement progress from saved data
    private void ReportAchievement(string idKey)
    {
        //PlayerPrefs.DeleteAll();
        Social.ReportProgress(idKey, PlayerPrefs.GetInt(idKey), success =>
        {
            if (success)
            {
                print("Success report: " + idKey);
            }
            else
            {
                print("Report failed: " + idKey);
            }
        });
    }

    // Show ahievement UI
    public void LoadAchievement()
    {
        Social.LoadAchievements(achievements =>
        {
            if (achievements.Length > 0)
            {
                int i = 0;

                Debug.Log("Got " + achievements.Length + " achievement instances");
                foreach (IAchievement achievement in achievements)
                {
                    string myAchievements = "My achievements:\n";
                    myAchievements += "\t" +
                        achievement.id + " " +
                        achievement.percentCompleted + " " +
                        achievement.completed + " " +
                        achievement.lastReportedDate + "\n";
                    Debug.Log(myAchievements);

                    // Display achievement to UI text
                    AchievementUI[i].text = " " + AchievementDescription[i] + ": " + achievement.percentCompleted + "%";
                    if (achievement.percentCompleted == 100)
                    {
                        AchievementUI[i].color = Color.yellow;
                    }
                    i++;
                    if (i > AchievementUI.Length - 1)
                    {
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("No achievements returned");
            }
        });
    }
}
