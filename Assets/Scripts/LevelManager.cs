using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform cameraPoint;
    public List<GameObject> objectsToDisable; //objects that needs to be disabled when the player isn't in the level

    /// <summary>
    /// Disable object in the Level when the player isn't in the level
    /// </summary>
    public void DisableLevel()
    {
        foreach (GameObject go in objectsToDisable)
        {
            go.SetActive(false);
        }
    }

    /// <summary>
    /// Enable object when the player is in this level
    /// </summary>
    public void EnableLevel()
    {
        foreach (GameObject go in objectsToDisable)
        {
            go.SetActive(true);
        }
    }
}
