using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

public class AchievementManager : MonoBehaviour
{

    public string[] achievementDescription;
    public Text[] achievementUI;

    // Use this for initialization
    void Start()
    {
        CreateAchievement();
    }

    // 
    public void CreateAchievement()
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
        //PlayerPrefs.DeleteKey(idKey);
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
        PlayerPrefs.SetInt(idKey, 100);
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
                    achievementUI[i].text = " " + achievementDescription[i] + ": " + achievement.percentCompleted + "%";
                    if (achievement.percentCompleted == 100)
                    {
                        achievementUI[i].color = Color.yellow;
                    }
                    i++;
                }
            }
            else
            {
                Debug.Log("No achievements returned");
            }
        });

        Social.ShowAchievementsUI();
    }
}
