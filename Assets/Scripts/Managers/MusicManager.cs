using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public List<SongManager> themes;

    public SongManager mainTheme;
    public SongManager currentPlayingTheme;

    void Start()
    {
        foreach (Transform child in transform)
        {
            themes.Add(child.GetComponent<SongManager>());
        }

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
            if (!theme.isMainTheme)
            {
                for (int i = 0; i < theme.levelsToPlayTheme.Count; i++)
                {
                    if (theme.levelsToPlayTheme[i] == newCurrentLevel)
                    {
                        if (theme != currentPlayingTheme) // If newCurrentLevel equals a level in which theme must be changed, change theme
                        {
                            SwitchTheme(theme);
                            break;
                        }
                    }
                    else
                    {
                        if (theme == currentPlayingTheme) // If not, resume to main theme
                        {
                            SwitchTheme(mainTheme);
                            break;
                        }
                    }
                }
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
        StartMusic(newTheme);
        ResumeMusic(newTheme);
    }

    public void StartMusic(SongManager songManager)
    {
        songManager.GetTheme().start();
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
