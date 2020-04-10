using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

public class DiscordController : Singleton<DiscordController>
{
    private static long CLIENT_ID = 680874741497987089;
    private Discord.Discord discord;
    private Discord.ActivityManager activityManager;
    private static DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private string state;
    private string details;
    private string smallImage;
    private string smallText;

    void Start()
    {
        discord = new Discord.Discord(CLIENT_ID, (System.UInt64)Discord.CreateFlags.Default);
        SetActivity();
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
            State = state,
            Details = details,
            Timestamps = new Discord.ActivityTimestamps
            {
                Start = (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds / 1000
            },
            Assets = new Discord.ActivityAssets
            {
                LargeImage = "default",
                LargeText = "From the Shadows",
                SmallImage = smallImage,
                SmallText = smallText
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

    public void SetActivity()
    {
        int chapter = GameManager.Instance.CurrentChapter;
        if (chapter >= 0)
        {
            switch (chapter)
            {
                case 0:
                    state = "Prologue";
                    break;
                case 1:
                    state = "Chapter 1";
                    break;
                case 2:
                    state = "Chapter 2";
                    break;
                default:
                    state = null;
                    break;
            }
        }
        else
        {
            state = "Main menu";
        }
        int save = GameManager.Instance.CurrentSave;
        if (save >= 0)
        {
            int nbPlayer = GameManager.Instance.Saves[GameManager.Instance.CurrentSave].NbPlayer;
            details = nbPlayer == 1 ? "Playing Solo" : "Playing Duo";
        }
        else
        {
            details = null;
        }
        UpdatePresence();
    }
}
