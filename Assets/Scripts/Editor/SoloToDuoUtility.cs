using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SoloToDuoUtility : MonoBehaviour
{
    public void SwitchToSolo()
    {
        Debug.Log("Switched to Solo Mode");

        // TODO : Change Player1 & Player2 to PlayerSolo
        // Change dialogues ? Or maybe just enable/disable new one in the Solo/Duo level prefab
        GameObject.Find("Players").transform.GetChild(0).gameObject.SetActive(true); // Enable SoloPlayers
        GameObject.Find("Players").transform.GetChild(1).gameObject.SetActive(false); // Disable DuoPlayers


        CameraTarget cameraTarget = FindObjectOfType<CameraTarget>();
        cameraTarget.isSolo = true; // Set the Camera Target to follow only one player
        EditorUtility.SetDirty(cameraTarget);


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
        Debug.Log("Switched to Duo Mode");


        GameObject.Find("Players").transform.GetChild(0).gameObject.SetActive(false); // Enable SoloPlayers
        GameObject.Find("Players").transform.GetChild(1).gameObject.SetActive(true); // Disable DuoPlayers


        CameraTarget cameraTarget = FindObjectOfType<CameraTarget>();
        cameraTarget.isSolo = false; // Set the Camera Target to follow only one player
        EditorUtility.SetDirty(cameraTarget);


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
