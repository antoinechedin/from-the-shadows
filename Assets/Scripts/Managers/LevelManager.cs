using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform cameraLimitLB;
    public Transform cameraLimitRT;

    public GameObject startSpawns;
    public GameObject endSpawns;

    public List<GameObject> collectibles;

    public List<GameObject> objectsToDisable; //objects that needs to be disabled when the player isn't in the level


    private void Awake()
    {
        //Handle spawn points potential problems
        if (startSpawns == null)
        {
            Debug.LogWarning(name + " : StartSpawns is not set. Pass the parent GameObject that contains the spawnPoints for the start of the level");
        }
        else if (startSpawns.transform.childCount != 2)
        {
            Debug.LogWarning(name + ".StartSpawns has to have 2 children.");
        }

        if (endSpawns == null)
        {
            Debug.LogWarning(name + " : EndSpawns is not set. Pass the parent GameObject that contains the spawnPoints for the end of the level");
        }
        else if (endSpawns.transform.childCount != 2)
        {
            Debug.LogWarning(name + ".EndSpawns has to have 2 children.");
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
            collectibles[i].GetComponent<Collectible>().isValidated = collectiblesTaken[i];
            collectibles[i].GetComponent<Collectible>().UpdateState();
        }
    }


}
