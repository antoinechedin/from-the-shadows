using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingKillZoneCP : MonoBehaviour
{
    public Transform targetPos;
    public Transform resetPoint;
    public float maxSpeed;
    public float timeToMaxSpeed;
    public bool mustReset;

    private MovingKillZone movingKillZone;
    private GameObject currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        movingKillZone = GameObject.FindObjectOfType<MovingKillZone>();
        currentLevel = GetComponentInParent<LevelManager>().gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            movingKillZone.SetNewInfos(this);
            movingKillZone.transform.SetParent(currentLevel.transform);

            if (mustReset)
            {
                movingKillZone.killZone.transform.position = resetPoint.position;
            }
        }
    }
}
