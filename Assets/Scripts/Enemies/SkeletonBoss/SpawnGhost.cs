using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGhost : MonoBehaviour, IResetable
{
    public GameObject ghost;
    public float minSecondsAfterRespawn = 3;
    public float maxSecondsAfterRespawn = 6;
    public Transform[] spawnPoints;
    public float minDistancePlayer;
    public Transform player1;
    public Transform player2;

    public void Start()
    {
    }

    public void StartSpawningGhost()
    {
        Invoke("Spawn", minSecondsAfterRespawn);
    }

    public void Spawn()
    {
        Vector3 spawnPoint = GetRandomSpawnPoint();
        GameObject go =Instantiate(ghost, spawnPoint, Quaternion.identity);
        go.transform.parent = transform;
        float randomTime = Random.Range(minSecondsAfterRespawn, maxSecondsAfterRespawn);
        Invoke("Spawn", randomTime);
    }

    public Vector3 GetRandomSpawnPoint()
    {        
        int random = Random.Range(0, spawnPoints.Length);
        if (Vector3.Distance(spawnPoints[random].position, player1.position) > minDistancePlayer 
         && Vector3.Distance(spawnPoints[random].position, player2.position) > minDistancePlayer)
        { 
            return spawnPoints[random].position;
        }
        else if( spawnPoints.Length > 2)
        {
            return GetRandomSpawnPoint();
        }
        else
        {
            return spawnPoints[0].position;
        }
    }

    public void Reset()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
