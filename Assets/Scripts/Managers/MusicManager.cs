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

    [Range(0, 100)]
    public int masterVolume = 50;

    public bool playOnAwake = false;
    public FMOD.Studio.Bus masterBus;


    private void Awake()
    {
        foreach (Transform child in transform)
        {
            themes.Add(child.GetComponent<SongManager>());
        }

        if (playOnAwake)
            StartTheme(mainTheme);
    }

    void Start()
    {
        masterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");

        // if(GameManager.Instance.LoadingChapterInfo == null && !playOnAwake)
        //     StartTheme(mainTheme);
    }

    private void Update()
    {
        SetMasterVolume(masterVolume);
    }

    // Set music according to the current level (in case of loading a specified level and not starting from the beginning)
    public void SetMusicAccordingToLevel(int loadedLevelId, List<LevelManager> levels)
    {
        Debug.Log("Music set to level " + loadedLevelId);

        bool mustPlayMainTheme = true;

        foreach(SongManager theme in themes)
        {
            foreach(LevelManager level in theme.levelsToPlayTheme)
            {
                if(loadedLevelId == level.id)
                {
                    Debug.Log("Must play theme : " + theme);
                    StartTheme(theme);
                    mustPlayMainTheme = false;
                    break;
                }
            }
            if (!mustPlayMainTheme)
                break;
        }

        mainTheme.SetLayerAccordingToLevel(loadedLevelId);

        if (mustPlayMainTheme)
        {
            Debug.Log("Must play main theme");
            StartTheme(mainTheme);
        }

        //foreach (LevelManager levelToAddLayer in levelsToAddLayer)
        //{
        //    if (level >= levelToAddLayer.id)
        //    {
        //        AddLayer();
        //    }
        //}
    }

    public void SetMasterVolume(float volume)
    {
        masterBus.setVolume(volume / 100f);
    }
    public void ManageMusicChange(int currentLevel, int newCurrentLevel)
    {
        bool themeUpdated = false;

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
                            themeUpdated = true;
                        }
                        switchToMainTheme = false;
                        break;
                    }
                    else
                    {
                        switchToMainTheme = true;
                    }
                }

                if (switchToMainTheme && (currentPlayingTheme != mainTheme) && (theme == currentPlayingTheme))
                    SwitchTheme(mainTheme);
            }

            if (themeUpdated)
                break;
        }
    }

    public void SwitchTheme(SongManager newTheme)
    {
        Debug.Log("Theme switched to " + newTheme);

        StopTheme(currentPlayingTheme, newTheme);
        StartTheme(newTheme);
        //PauseTheme(currentPlayingTheme);

        //if (!newTheme.hasStarted)
        //    StartTheme(newTheme);
        //else
        //    ResumeTheme(newTheme);

        currentPlayingTheme = newTheme;
    }

    public void StartTheme(SongManager songManager)
    {
        songManager.GetTheme().start();
        currentPlayingTheme = songManager;
    }

    // Stop specified theme to start another
    public void StopTheme(SongManager songManager, SongManager newTheme)
    {
        songManager.GetTheme().stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        currentPlayingTheme = newTheme;
    }

    // Stop current playing theme
    public void StopTheme()
    {
        currentPlayingTheme.GetTheme().stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        currentPlayingTheme = null;
    }

    public void PauseTheme(SongManager songManager)
    {
        songManager.GetTheme().setPaused(true);
    }

    public void ResumeTheme(SongManager songManager)
    {
        songManager.GetTheme().setPaused(false);
    }

}
