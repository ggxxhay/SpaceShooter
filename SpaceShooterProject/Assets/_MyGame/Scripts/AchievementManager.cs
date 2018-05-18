using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

public class AchievementManager : MonoBehaviour
{
    public static string[] AchievementDescription;

    // Point can receive when complete achievement
    private int[] rewardPoint = new int[] { 5, 20, 100 };
    // Define if reward was received or not
    private int[] isRewardReceived;
    private string[] rewardReceiveKeys = new string[] { "Reward1", "Reward2", "Reward3" };
    public GameObject[] rewardButtons;

    public Text[] AchievementUI;
    public GameObject NoticeText;

    // Use this for initialization
    void Start()
    {
        InitializeData();

        LoadRewardStatus();

        CreateAchievement();
    }

    /// <summary>
    /// Setup game objects: ahicevement description, reward buttons
    /// </summary>
    private void InitializeData()
    {
        AchievementDescription = new string[]
        {
            "Kill first enemy",
            "Kill 10 enemies",
            "Kill 100 enemies"
        };

        NoticeText.SetActive(false);

        //rewardButtons = new GameObject[]
        //{
        //    GameObject.Find("Reward"),
        //    GameObject.Find("Reward1"),
        //    GameObject.Find("Reward2")
        //};
    }

    /// <summary>
    /// Load status received or not of rewards
    /// </summary>
    private void LoadRewardStatus()
    {
        isRewardReceived = new int[] { 0, 0, 0 };

        for (int i = 0; i < rewardPoint.Length; i++)
        {
            isRewardReceived[i] = PlayerPrefs.GetInt(rewardReceiveKeys[i]);
        }
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

    /// <summary>
    /// Show ahievement UI
    /// </summary>
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
                    //AchievementUI[i].text = " " + AchievementDescription[i] + ": " + achievement.percentCompleted + "%";
                    if (achievement.percentCompleted == 100)
                    {
                        AchievementUI[i].color = Color.yellow;
                    }
                    LoadRewardButton(i, achievement.percentCompleted);
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

    /// <summary>
    /// Load status of reward button.
    /// </summary>
    /// <param name="buttinIndex">Index of button</param>
    /// <param name="percentCompleted">Percent completed of achivement</param>
    private void LoadRewardButton(int buttinIndex, double percentCompleted)
    {
        if (percentCompleted == 100 && isRewardReceived[buttinIndex] == 0)
        {
            rewardButtons[buttinIndex].SetActive(true);
        }
        else
        {
            rewardButtons[buttinIndex].SetActive(false);
        }
    }

    /// <summary>
    /// Receive reward when user click on reward button.
    /// </summary>
    public void RewardButtonClick(int buttonIndex)
    {
        // Receive and save reward.
        int currentPoint = PlayerPrefs.GetInt(Keys.totalScoreKey);
        PlayerPrefs.SetInt(Keys.totalScoreKey, currentPoint + rewardPoint[buttonIndex]);

        StartCoroutine(FindObjectOfType<LevelManager>().
            Notify(NoticeText, "You have received " + rewardPoint[buttonIndex] + " gold"));

        // Change reward and button information.
        rewardButtons[buttonIndex].SetActive(false);
        isRewardReceived[buttonIndex] = 1;
        SaveRewardStatus();
    }

    /// <summary>
    /// Save status received or not of rewards.
    /// </summary>
    private void SaveRewardStatus()
    {
        for (int i = 0; i < rewardPoint.Length; i++)
        {
            PlayerPrefs.SetInt(rewardReceiveKeys[i], isRewardReceived[i]);
        }
    }
}
