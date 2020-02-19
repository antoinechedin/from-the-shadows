using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform cameraLimitLB;
    public Transform cameraLimitRT;

    public List<Transform> spawnPoints;

    public Transform lightSpawn;
    public Transform shadowSpawn;
    public List<GameObject> collectibles;

    public List<GameObject> objectsToDisable; //objects that needs to be disabled when the player isn't in the level

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

        //Handle spawn points not set
        if (lightSpawn == null)
        {
            Debug.LogWarning(name +" : lightSpawn is not set. Spawning at (0, 0, 0) by defaut.");

            GameObject defaultLightSpawn =  new GameObject("defaultLightSpawn");
            defaultLightSpawn.AddComponent<Transform>();
            defaultLightSpawn.transform.position = Vector3.zero;
            lightSpawn = defaultLightSpawn.transform;
        }

        if (shadowSpawn == null)
        {
            Debug.LogWarning(name + " : shadowSpawn is not set. Spawning at (0, 0, 0) by defaut.");

            GameObject defaultShadowSpawn = new GameObject("defaultShadowPoint");
            defaultShadowSpawn.AddComponent<Transform>();
            defaultShadowSpawn.transform.position = Vector3.zero;
            shadowSpawn = defaultShadowSpawn.transform;
        }

        if (shadowSpawn == lightSpawn)
        {
            Debug.LogWarning(name + " : Light and Shadow spawns are the same. There is no problem but you might want to set different ones.");

            GameObject defaultShadowSpawn = new GameObject("defaultShadowPoint");
            defaultShadowSpawn.AddComponent<Transform>();
            defaultShadowSpawn.transform.position = Vector3.zero;
            shadowSpawn = defaultShadowSpawn.transform;
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
