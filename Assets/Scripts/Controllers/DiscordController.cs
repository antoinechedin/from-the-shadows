using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

public class DiscordController : MonoBehaviour
{
    private static long CLIENT_ID = 680874741497987089;

    public Discord.Discord discord;
    private Discord.ActivityManager activityManager;
    private DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    void Start()
    {
        discord = new Discord.Discord(CLIENT_ID, (System.UInt64)Discord.CreateFlags.Default);
        UpdatePresence();
    }

    void Update()
    {
        discord.RunCallbacks();
    }

    void UpdatePresence()
    {
        activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            State = "Chapter 1",
            Details = "Playing Solo",
            Timestamps = new Discord.ActivityTimestamps
            {
                Start = (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds / 1000
            },
            Assets = new Discord.ActivityAssets
            {
                LargeImage = "default",
                LargeText = "From the Shadows",
                SmallImage = "default",
                SmallText = "Chapter 1"
            }
        };
        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res == Discord.Result.Ok)
            {
                Debug.Log("Success!");
            }
            else
            {
                Debug.LogError("Failed");
            }
        });
    }

    public Discord.ActivityManager ActivityManager
    {
        get => activityManager;
        set => activityManager = value;
    }
}
