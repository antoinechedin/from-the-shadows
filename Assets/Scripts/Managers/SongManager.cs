using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SongManager : MonoBehaviour
{
    [SerializeField]
    [FMODUnity.EventRef]
    private string _theme = "";
    private FMOD.Studio.EventInstance theme;

    public bool isMainTheme = false;
    [Header("Levels in which the theme is played. Only relevant if not Main Theme")]
    public List<int> levelsToPlayTheme;

    [Header("Levels in which layer appear. For Layered theme only")]
    public bool isLayered = false;
    private int musicLayer = 0;
    public List<int> levelsToAddLayer;

    // Start is called before the first frame update
    void Start()
    {
        theme = FMODUnity.RuntimeManager.CreateInstance(_theme);
    }


    // Add Music Layer to the current music
    public void AddLayer()
    {
        musicLayer++;
        theme.setParameterByName("Layer" + musicLayer, 1f);
        Debug.Log("Added layer : now layer " + musicLayer);
    }

    // Remove Music Layer to the current music
    public void RemoveLayer()
    {
        theme.setParameterByName("Layer" + musicLayer, 0f);
        musicLayer--;
        Debug.Log("Removed layer : now layer " + musicLayer);
    }

    // Set Music Layers according to the current level
    public void SetLayerAccordingToLevel(int level)
    {
        foreach (int levelToAddLayer in levelsToAddLayer)
        {
            if (level >= levelToAddLayer)
            {
                AddLayer();
            }
        }
    }

    public int GetLevelToAddLayer()
    {
        if(musicLayer < levelsToAddLayer.Count)
            return levelsToAddLayer[musicLayer];
        return -1;
    }

    public int GetLevelToRemoveLayer()
    {
        return levelsToAddLayer[musicLayer - 1];
    }

    public FMOD.Studio.EventInstance GetTheme()
    {
        return theme;
    }

}

