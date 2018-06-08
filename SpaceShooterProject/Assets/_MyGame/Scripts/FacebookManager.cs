using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System;


public class FacebookManager : MonoBehaviour {

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init();
        }
        else
        {
            FB.ActivateApp();
        }
    }

    public void Share()
    {
        // Title and description cannot set bu custom, the post will use link's title and description to display
        FB.ShareLink(
            contentURL: new Uri("https://facebook.com"),
            callback: OnShare
            );
    }

    private void OnShare(IShareResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
        {
            print("Share Error: "+result.Error);
        }else if (!string.IsNullOrEmpty(result.PostId))
        {
            print("Post ID: "+result.PostId);
        }
        else
        {
            print("Share succeed!");
        }
    }
}
