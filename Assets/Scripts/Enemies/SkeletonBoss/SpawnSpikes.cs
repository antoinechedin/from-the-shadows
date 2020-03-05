using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpikes : MonoBehaviour
{
    public Transform[] points; 
    public GameObject prefab;
    public float speed = 5;
    public float minSecondsAfterRespawn = 3;
    public float maxSecondsAfterRespawn = 6;

    private GameObject go;
    private Vector3 from;
    private Vector3 to;
    private int randomLane = -1;
    private int randomDirection = -1;
    private bool forceChangeLane = false;
    private bool rotate = false;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Spawn", minSecondsAfterRespawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (go != null)
            go.transform.position = Vector3.MoveTowards(go.transform.position, to, Time.deltaTime * speed);
        if (go != null && go.transform.position == to)
        {            
            Destroy(go);
            go = null;
            float randomTime = Random.Range(minSecondsAfterRespawn, maxSecondsAfterRespawn);
            Invoke("Spawn", randomTime);
        }
    }

    public void Spawn()
    {
        // Choose a random lane
        int random = Random.Range(0, points.Length / 2);

        // If the lane do not change, we only change the direction
        // if it is the same lane for the 3rd time we re-execute the funtion until it changes
        if( random == randomLane)
        {
            if (forceChangeLane)
            {
                Spawn();
                return;
            }
        }
        else
        {
            // Else we set the new lane
            forceChangeLane = false;
            randomLane = random;
            from = points[randomLane * 2].position;
            to = points[randomLane * 2 + 1].position;

            // and choose a random direction
            randomDirection = Random.Range(0, 2);
            if (randomDirection == 1)
            {
                SwitchDirection();                
            }
        }

        // Spawn the prefab
        if (from != null && to != null && prefab != null)
            go = Instantiate(prefab, from, Quaternion.identity);

        if (rotate)
        {
            go.transform.localScale = new Vector3(-1, 1, 1);
        }
        rotate = false;
    }

    public void SwitchDirection()
    {
        Vector3 tmp = from;
        from = to;
        to = tmp;
        rotate = true;
    }
}
