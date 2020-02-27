using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
#if UNITY_EDITOR
    [ReadOnly]
#endif
    [Header("The id will automatically be set by the ChapterManager")]
    public int id;
    public CinemachineVirtualCamera virtualCamera;
    public BoxCollider2D levelLimits;

    public List<GameObject> lightCollectibles = new List<GameObject>();
    public List<GameObject> shadowCollectibles = new List<GameObject>();
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
                if (parentCollectibles.GetChild(i).GetComponent<Collectible>().type == Collectible.Type.Light)
                {
                    lightCollectibles.Add(parentCollectibles.GetChild(i).gameObject);
                }
                else if (parentCollectibles.GetChild(i).GetComponent<Collectible>().type == Collectible.Type.Shadow)
                {
                    shadowCollectibles.Add(parentCollectibles.GetChild(i).gameObject);
                }

            }
        }

        if (virtualCamera == null)
        {
            GameObject defaultVirtualCamera = Instantiate(new GameObject("Virtual Camera"), transform);
            defaultVirtualCamera.AddComponent<CinemachineVirtualCamera>();
            defaultVirtualCamera.AddComponent<CinemachineConfiner>();
            virtualCamera = defaultVirtualCamera.GetComponent<CinemachineVirtualCamera>();
        }

        if (levelLimits == null)
        {
            GameObject defaultLevelLimits = Instantiate(new GameObject("Level limits"), transform);
            defaultLevelLimits.AddComponent<BoxCollider2D>();
            levelLimits = defaultLevelLimits.GetComponent<BoxCollider2D>();
        }

        GameObject cameraConfiner = Instantiate(new GameObject("Camera Confiner"), transform);
        cameraConfiner.AddComponent<BoxCollider>();

        Camera.main.GetComponent<CameraManager>().ProcessCameraConfiner(levelLimits, virtualCamera, cameraConfiner.GetComponent<BoxCollider>());
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

    public void SetCollectibles(bool[] lightCollectiblesTaken, bool[] shadowCollectibleTaken)
    {
        for (int i = 0; i < lightCollectibles.Count; i++)
        {
            if (i < lightCollectiblesTaken.Length)
            {
                if (lightCollectibles[i].GetComponent<Collectible>() != null)
                {
                    lightCollectibles[i].GetComponent<Collectible>().isValidated = lightCollectiblesTaken[i];
                    lightCollectibles[i].GetComponent<Collectible>().UpdateState();
                }
            }
        }

        for (int i = 0; i < shadowCollectibles.Count; i++)
        {
            if (i < shadowCollectibleTaken.Length)
            {
                if (shadowCollectibles[i].GetComponent<Collectible>() != null)
                {
                    shadowCollectibles[i].GetComponent<Collectible>().isValidated = shadowCollectibleTaken[i];
                    shadowCollectibles[i].GetComponent<Collectible>().UpdateState();
                }
            }
        }
    }


}
