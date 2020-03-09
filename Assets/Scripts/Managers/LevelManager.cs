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
    public float maxCameraDepth = 35f;
    public Vector2 cameraOffset;

    public List<GameObject> lightCollectibles = new List<GameObject>();
    public List<GameObject> shadowCollectibles = new List<GameObject>();
    public List<GameObject> playerSpawns;

    public List<GameObject> objectsToDisable; //objects that needs to be disabled when the player isn't in the level
    [Header("Put in roomsToEnable all the neighbors of the room")]
    public List<LevelManager> roomsToEnable;

    private BoxCollider2D levelLimits;
    private GameObject cameraConfiner;

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

        // Crée une camera par défaut si aucune n'est renseignée. Préférez référencer celle en prefab

        if (virtualCamera == null)
        {
            GameObject defaultVirtualCamera = new GameObject("Virtual Camera");
            defaultVirtualCamera.AddComponent<CinemachineVirtualCamera>();
            defaultVirtualCamera.GetComponent<CinemachineVirtualCamera>().AddCinemachineComponent<CinemachineFramingTransposer>();

            defaultVirtualCamera.GetComponent<CinemachineVirtualCamera>().AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            CinemachineBasicMultiChannelPerlin noise = defaultVirtualCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_NoiseProfile = Resources.Load("CustomShakeNoise") as NoiseSettings;
            noise.m_AmplitudeGain = 0f;

            defaultVirtualCamera.AddComponent<CinemachineConfiner>();
            defaultVirtualCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 20;
            defaultVirtualCamera.GetComponent<CinemachineConfiner>().m_ConfineMode = CinemachineConfiner.Mode.Confine3D;
            Vector3 rotationVector = defaultVirtualCamera.transform.rotation.eulerAngles;
            rotationVector.x = 6;
            defaultVirtualCamera.transform.rotation = Quaternion.Euler(rotationVector);
            virtualCamera = defaultVirtualCamera.GetComponent<CinemachineVirtualCamera>();
            virtualCamera.gameObject.SetActive(false);
            virtualCamera.transform.SetParent(transform);
            Debug.Log("Virtual Camera is not set. Create a default one.");
        }
        else
        {
            virtualCamera = Instantiate(virtualCamera, transform);
            virtualCamera.gameObject.SetActive(false);
        }

        levelLimits = gameObject.GetComponent<BoxCollider2D>();

        if (levelLimits == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
            levelLimits = gameObject.GetComponent<BoxCollider2D>();
            Debug.LogWarning("Levels limits is not set.");
        }

        cameraConfiner = new GameObject("Camera Confiner");
        cameraConfiner.AddComponent<BoxCollider>();
        cameraConfiner.transform.SetParent(transform);
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

        Camera.main.GetComponent<CameraManager>().ProcessCameraConfiner(levelLimits, virtualCamera, cameraConfiner.GetComponent<BoxCollider>(), maxCameraDepth);
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
