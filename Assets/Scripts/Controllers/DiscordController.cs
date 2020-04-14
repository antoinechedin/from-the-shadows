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

    public void Init()
    {
        if (discord == null)
        {
            discord = new Discord.Discord(CLIENT_ID, (System.UInt64)Discord.CreateFlags.Default);
            SetActivity();
        }
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
        if (GameObject.FindObjectOfType<MenuManager>() != null)
        {
            details = "Main menu";
            state = null;
            smallImage = "mainmenu";
            smallText = details;
        }
        else
        {
            int chapter = GameManager.Instance.CurrentChapter;
            switch (chapter)
            {
                case 0:
                    details = "Prologue";
                    smallImage = "chapter0";
                    break;
                case 1:
                    details = "Chapter 1";
                    smallImage = "chapter1";
                    break;
                case 2:
                    details = "Chapter 2";
                    smallImage = "chapter2";
                    break;
                default:
                    details = "Main menu";
                    smallImage = "mainmenu";
                    break;
            }
            int save = GameManager.Instance.CurrentSave;
            if (save >= 0)
            {
                int nbPlayer = GameManager.Instance.Saves[GameManager.Instance.CurrentSave].NbPlayer;
                state = nbPlayer == 1 ? "Playing Solo" : "Playing Duo";
            }
            else
            {
                state = null;
            }
            smallText = details;
        }
        UpdatePresence();
    }
}
