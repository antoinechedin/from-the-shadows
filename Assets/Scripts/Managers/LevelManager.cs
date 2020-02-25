using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #if UNITY_EDITOR
    [ReadOnly]
    #endif
    [Header("The id will automatically be set by the ChapterManager")]
    public int id;
    public Transform cameraLimitLB;
    public Transform cameraLimitRT;

    public List<GameObject> collectibles = new List<GameObject>();
    public List<GameObject> playerSpawns;

    public List<GameObject> objectsToDisable; //objects that needs to be disabled when the player isn't in the level
    [Header("Put in roomsToEnable all the neighbors of the room")]
    public List<LevelManager> roomsToEnable;


    private void Awake()
    {
        //Handle spawn points potential problems
        if (playerSpawns == null || playerSpawns.Count == 0)
        {
            Debug.LogWarning("playerSpawns list is not set");
        }
        else
        {
            foreach (GameObject go in playerSpawns)
            {
                if (go.transform.childCount != 2)
                {
                    Debug.LogWarning("playerSpawns." + go.name + " : needs to have 2 children.");
                }
            }
        }
    }
    public void Start()
    {
        // Fetch collectibles
        Transform parentCollectibles = transform.Find("Collectibles");
        if (parentCollectibles != null)
        {
            for (int i = 0; i < parentCollectibles.childCount; i++)
            {
                collectibles.Add(parentCollectibles.GetChild(i).gameObject);
            }
        }
    }
    /// <summary>
    /// Disable object in the Level when the player isn't in the level
    /// </summary>
    public void DisableLevel()
    {
        gameObject.SetActive(false);
        foreach (LevelManager level in roomsToEnable)
        {
            if (level != null)
            {
                level.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Enable object when the player is in this level
    /// </summary>
    public void EnableLevel()
    {
        gameObject.SetActive(true);
        this.SetObjectToDisable(true);

        foreach (LevelManager level in roomsToEnable)
        {
            if (level != null)
            {
                level.gameObject.SetActive(true);
                level.SetObjectToDisable(false);
            }
        }
    }

    public void SetObjectToDisable(bool b)
    {
        foreach (GameObject go in objectsToDisable)
        {
            go.SetActive(b);
        }
    }

    public void ResetAllResetables()
    {
        foreach (IResetable resetable in GetComponentsInChildren<IResetable>())
        {
            resetable.Reset();
        }
    }

    public void SetCollectibles(bool[] collectiblesTaken)
    {
        for (int i = 0; i < collectibles.Count; i++)
        {
            if (i < collectiblesTaken.Length)
            {
                if (collectibles[i].GetComponent<Collectible>() != null)
                {
                    collectibles[i].GetComponent<Collectible>().isValidated = collectiblesTaken[i];
                    collectibles[i].GetComponent<Collectible>().UpdateState();
                }
            }
        }
    }


}
