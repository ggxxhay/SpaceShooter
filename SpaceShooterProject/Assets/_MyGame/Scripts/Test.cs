using UnityEngine;
using UnityEngine.UI;
using System;

public class Test : MonoBehaviour
{
    public int AlarmRepeatInterval;
    public int[] AlarmTimeDetails;

    static public bool isAlarmRunning;

#if UNITY_ANDROID
    AndroidJavaClass androidClass;
    AndroidJavaObject currentActivity;
#endif

    // Use this for initialization
    void Start()
    {
        isAlarmRunning = false;

#if UNITY_ANDROID
        // Get MainActivity instance
        androidClass = new AndroidJavaClass("com.example.ggxxh.notificationpluginidea.MainActivity");

        // Get current Activity.
        currentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").
            GetStatic<AndroidJavaObject>("currentActivity");
#endif
    }

    public void SetAlarmDetails()
    {
        InputField repeatInterval = GameObject.Find("RepeatInterval").GetComponent<InputField>();
        InputField alarmHour = GameObject.Find("AlarmHour").GetComponent<InputField>();
        InputField alarmMinute = GameObject.Find("AlarmMinute").GetComponent<InputField>();
        InputField fromHour = GameObject.Find("FromHour").GetComponent<InputField>();
        InputField fromMinute = GameObject.Find("FromMinute").GetComponent<InputField>();
        InputField toHour = GameObject.Find("ToHour").GetComponent<InputField>();
        InputField toMinute = GameObject.Find("ToMinute").GetComponent<InputField>();
        try
        {
            AlarmRepeatInterval = Convert.ToInt32(repeatInterval.text);
            AlarmTimeDetails = new int[]
            {
            Convert.ToInt32(alarmHour.text),
            Convert.ToInt32(alarmMinute.text),
            Convert.ToInt32(fromHour.text),
            Convert.ToInt32(fromMinute.text),
            Convert.ToInt32(toHour.text),
            Convert.ToInt32(toMinute.text),
            };
        }
        catch (FormatException ex)
        {
            Debug.Log(ex);
            AlarmRepeatInterval = 5;
            AlarmTimeDetails = new int[6] { 11, 45, 8, 0, 17, 30 };
        }
        NotifyUser();
        FindObjectOfType<LevelManager>().ToMainMenu();
    }

    public void NotifyUser()
    {
#if UNITY_ANDROID
        // Get a MainActivity instance.
        AndroidJavaObject javaObject = androidClass.CallStatic<AndroidJavaObject>("GetInstance");
        javaObject.Call("SchedulingNotify",
                        currentActivity,
                        isAlarmRunning,
                        "Hello Android Oreo 8.1",
                        "This is new description for the notification",
                        AlarmTimeDetails,
                        AlarmRepeatInterval
                        );
#endif
        // Change the text of alarm button by the state of the alarm
        GameObject setAlarmButton = GameObject.Find("SetAlarm");
        if (isAlarmRunning)
        {
            isAlarmRunning = false;
            setAlarmButton.GetComponent<Text>().text = "Set Alarm";
        }
        else
        {
            isAlarmRunning = true;
            setAlarmButton.GetComponent<Text>().text = "Cancel Alarm";
        }
    }

    /// <summary>
    /// Check if the game is access from the notification or not
    /// </summary>
    /// <returns></returns>
    public bool CheckCallBack()
    {
#if UNITY_ANDROID
        AndroidJavaObject javaObject = androidClass.CallStatic<AndroidJavaObject>("GetInstance");
        return javaObject.Call<bool>("IsAccessFromNotification", currentActivity);
#endif
    }
}
