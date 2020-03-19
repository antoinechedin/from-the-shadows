using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SoloToDuoUtility : MonoBehaviour
{
    public void SwitchToSolo()
    {
        Debug.Log("solo");

        // TODO : Change Player1 & Player2 to PlayerSolo
        // Change Camera for Solo

        foreach (LevelManager level in FindObjectsOfType<LevelManager>()) // Enable the solo version and disable the duo version
        {
            if(level.soloLevel != null && level.duoLevel != null)
            {
                level.soloLevel.SetActive(true);
                level.duoLevel.SetActive(false);
            }
        }

        foreach (ChangeLevelTrigger levelTrigger in FindObjectsOfType<ChangeLevelTrigger>()) // Set required player to change level to 1
        {
            levelTrigger.nbNecessaryPlayers = 1;
            EditorUtility.SetDirty(levelTrigger);
        }
    }

    public void SwitchToDuo()
    {
        Debug.Log("duo");

        foreach (LevelManager level in FindObjectsOfType<LevelManager>())
        {
            if (level.soloLevel != null && level.duoLevel != null)
            {
                level.soloLevel.SetActive(false);
                level.duoLevel.SetActive(true);
            }
        }

        foreach (ChangeLevelTrigger levelTrigger in FindObjectsOfType<ChangeLevelTrigger>())
        {
            levelTrigger.nbNecessaryPlayers = 2;
            EditorUtility.SetDirty(levelTrigger);
        }
    }
}
