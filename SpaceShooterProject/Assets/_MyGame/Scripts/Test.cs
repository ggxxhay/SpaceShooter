using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{

    public Text displayText;
    public int NotificationWaitInSeconds;

#if UNITY_ANDROID
    AndroidJavaClass androidClass;
    AndroidJavaObject currentActivity;
#endif

    // Use this for initialization
    void Start()
    {
        NotificationWaitInSeconds = 5;

#if UNITY_ANDROID
        // Get MainActivity instance
        androidClass = new AndroidJavaClass("com.example.ggxxh.notificationpluginidea.MainActivity");

        // Get current Activity.
        currentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").
            GetStatic<AndroidJavaObject>("currentActivity");
#endif
    }

    public void ButtonClicked()
    {
        //StartCoroutine(ButtonClick());
#if UNITY_ANDROID
        // Get a MainActivity instance.
        AndroidJavaObject javaObject = androidClass.CallStatic<AndroidJavaObject>("GetInstance");
        //androidClass.CallStatic("SetCurrentActivity", currentActivity);
        //AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
        javaObject.Call("SendNotification", 1, "Title", "Content", 3, currentActivity);

        javaObject.Call("SetCurrentActivity", currentActivity);
        javaObject.Call("SchedulingNotify", currentActivity, 
            NotificationWaitInSeconds, "New title setup", "This is new description for the notification");
#else
        Debug.Log("Not in android");
#endif
    }

    public IEnumerator ButtonClick()
    {
        displayText.text = "Begin";
        yield return new WaitForSeconds(2);

        yield return new WaitForSeconds(2);
        displayText.text = "End";
    }

    /// <summary>
    /// Check if the game is access from the notification or not
    /// </summary>
    /// <returns></returns>
    public bool CheckCallBack()
    {
#if UNITY_ANDROID
        // Get current Activity.
        //AndroidJavaObject currentActivity2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
        //    .GetStatic<AndroidJavaObject>("currentActivity");
        return androidClass.CallStatic<AndroidJavaObject>("GetInstance").Call<bool>("IsAccessFromNotification", currentActivity);
#endif
    }
}
