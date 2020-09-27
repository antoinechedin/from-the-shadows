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
        GameManager.Instance.Loading = true;
        gameObject.SetActive(true);
        _listeningKey = false;
        ChapterManager = chapterManager;
        animationPlayer.clip = animationIn;
        animationPlayer.Play();
        
        chapterManager.ValidateCollectibles();
        GameManager.Instance.SetLevelCompleted(GameManager.Instance.CurrentChapter, chapterManager.currentLevel);

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

    public void StartListeningKey()
    {
        _listeningKey = true;
    }

    private void Update()
    {
        if (!_listeningKey) return;

        if (InputManager.GetActionPressed(0, InputAction.Select)
            || Input.GetKeyDown(KeyCode.Escape)
            || Input.GetKeyDown(KeyCode.Backspace)
            || Input.GetKeyDown(KeyCode.Space))
        {
            _listeningKey = false;
            animationPlayer.Stop();
            animationPlayer.clip = animationOut;
            animationPlayer.Play();
            GetComponent<AudioSource>().PlayOneShot(ChapterManager.pauseMenu.uiPress);
        }
    }

    public void OnAnimationOutEnd()
    {
        ChapterManager.FinishChapter();
    }
}