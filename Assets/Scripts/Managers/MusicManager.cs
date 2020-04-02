using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private List<SongManager> themes;

    [SerializeField]
    private SongManager currentPlayingTheme;

    public SongManager mainTheme;


    private void Awake()
    {
        foreach (Transform child in transform)
        {
            themes.Add(child.GetComponent<SongManager>());
        }
    }

    void Start()
    {
        StartMusic(mainTheme);
    }

    public void ManageMusicChange(int currentLevel, int newCurrentLevel)
    {
        foreach (SongManager theme in themes)
        {
            // First check if we must change layer
            if (currentPlayingTheme.isLayered)
            {
                if (newCurrentLevel < currentLevel) // If we're going back
                {
                    if (currentPlayingTheme.GetLevelToRemoveLayer() == currentLevel) // If we changed layed in the current level
                    {
                        currentPlayingTheme.RemoveLayer(); // We remove a layer
                        break;
                    }
                }
                else if (currentPlayingTheme.GetLevelToAddLayer() == newCurrentLevel) // If we're going forward, if newCurrentLevel must add a layer
                {
                    currentPlayingTheme.AddLayer(); // We add a layer
                    break;
                }
            }

            // If not, check if we must change theme
            if (theme != mainTheme)
            {
                bool switchToMainTheme = false;

                for (int i = 0; i < theme.levelsToPlayTheme.Count; i++)
                {
                    if (theme.levelsToPlayTheme[i].id == newCurrentLevel) // If newCurrentLevel equals a level in which theme must be changed
                    {
                        if (theme != currentPlayingTheme) // If the theme is not playing, play it
                        {
                            SwitchTheme(theme);
                        }
                        switchToMainTheme = false;
                        break;
                    }
                    else
                    {
                        switchToMainTheme = true;
                    }
                }

                if (switchToMainTheme)
                    SwitchTheme(mainTheme);
            }
        }
    }

    /*
     * TODO :
     * Add smooth transition between two music
     */
    public void SwitchTheme(SongManager newTheme)
    {
        Debug.Log("Theme switched to " + newTheme);

        PauseMusic(currentPlayingTheme);

        if(!newTheme.hasStarted)
            StartMusic(newTheme);
        else
            ResumeMusic(newTheme);

        currentPlayingTheme = newTheme;
    }

    public void StartMusic(SongManager songManager)
    {
        songManager.GetTheme().start();
        songManager.hasStarted = true;
        currentPlayingTheme = songManager;
    }

    public void PauseMusic(SongManager songManager)
    {
        songManager.GetTheme().setPaused(true);
    }

    public void ResumeMusic(SongManager songManager)
    {
        songManager.GetTheme().setPaused(false);
    }

}
