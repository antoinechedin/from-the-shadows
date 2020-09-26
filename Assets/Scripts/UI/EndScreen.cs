using System;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    public ChapterManager ChapterManager { get; set; }

    public Animation animationPlayer;

    public AnimationClip animationIn;
    public AnimationClip animationOut;

    public TextMeshProUGUI timer;

    private bool _listeningKey;

    public void StartAnimationIn(ChapterManager chapterManager)
    {
        gameObject.SetActive(true);
        _listeningKey = false;
        ChapterManager = chapterManager;
        animationPlayer.clip = animationIn;
        animationPlayer.Play();

        if (PlayerPrefs.GetInt("SpeedRun") == 1)
        {
            float time = chapterManager.totalTimePlayed + chapterManager.timeSinceBegin;
            int secondes = Mathf.FloorToInt(time) % 60;
            int minutes = Mathf.FloorToInt(time) / 60 % 60;
            int hours = Mathf.FloorToInt(time) / 3600;
            int mili = Mathf.FloorToInt(time * 1000) % 1000;
            timer.SetText(
                hours.ToString("00") + ":"
                                     + minutes.ToString("00") + ":"
                                     + secondes.ToString("00") + "."
                                     + mili.ToString("000"));
        }
        else
        {
            timer.color = new Color(0f,0f,0f,0f);
        }
    }

    public void StartListeningKey()
    {
        _listeningKey = true;
    }

    private void Update()
    {
        if (!_listeningKey) return;

        _listeningKey = false;
        if (InputManager.GetActionPressed(0, InputAction.Select)
            || Input.GetKeyDown(KeyCode.Escape)
            || Input.GetKeyDown(KeyCode.Backspace))
        {
            animationPlayer.Stop();
            animationPlayer.clip = animationOut;
            animationPlayer.Play();
        }
    }

    public void OnAnimationOutEnd()
    {
        ChapterManager.FinishChapter();
    }
}