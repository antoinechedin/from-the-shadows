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

    [Header("Levels in which the theme is played. Only relevant if not Main Theme")]
    public List<LevelManager> levelsToPlayTheme;

    [Header("Levels in which layer appear. For Layered theme only")]
    public bool isLayered = false;
    private int musicLayer = 0;
    public List<LevelManager> levelsToAddLayer;


    public float layerVolume = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        theme = FMODUnity.RuntimeManager.CreateInstance(_theme);
    }

    private void LerpVolumeUp()
    {
        layerVolume += 0.05f;
        theme.setParameterByName("Layer" + musicLayer, layerVolume);

        if(layerVolume >= 1f)
        {
            theme.setParameterByName("Layer" + musicLayer, 1f);
            CancelInvoke("LerpVolumeUp");
        }
    }

    private void LerpVolumeDown()
    {
        layerVolume -= 0.05f;
        theme.setParameterByName("Layer" + (musicLayer + 1 ), layerVolume);

        if (layerVolume <= 0f)
        {
            theme.setParameterByName("Layer" + (musicLayer + 1), 0f);
            CancelInvoke("LerpVolumeDown");
        }

    }


    // Add Music Layer to the current music
    public void AddLayer()
    {
        musicLayer++;

        layerVolume = 0f;
        InvokeRepeating("LerpVolumeUp", 0f, 0.1f);

        Debug.Log("Added layer : now layer " + musicLayer);
    }

    // Remove Music Layer to the current music
    public void RemoveLayer()
    {
        musicLayer--;

        layerVolume = 1f;
        InvokeRepeating("LerpVolumeDown", 0f, 0.1f);

        Debug.Log("Removed layer : now layer " + musicLayer);
    }

    // Set Music Layers according to the current level
    public void SetLayerAccordingToLevel(int level)
    {
        foreach (LevelManager levelToAddLayer in levelsToAddLayer)
        {
            if (level >= levelToAddLayer.id)
            {
                AddLayer();
            }
        }
    }

    public int GetLevelToAddLayer()
    {
        if(musicLayer < levelsToAddLayer.Count)
            return levelsToAddLayer[musicLayer].id;
        return -1;
    }

    public int GetLevelToRemoveLayer()
    {
        if(musicLayer > 0)
            return levelsToAddLayer[musicLayer - 1].id;
        return -1;
    }

    public FMOD.Studio.EventInstance GetTheme()
    {
        return theme;
    }

}

