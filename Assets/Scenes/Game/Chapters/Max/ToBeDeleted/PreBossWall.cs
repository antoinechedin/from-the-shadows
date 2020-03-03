using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreBossWall : MonoBehaviour, IResetable
{
    public GameObject trigger;

    public bool isHorizontal = true;
    public bool isQuaranteCinqDegre = false;

    public float startingSpeed = 2f;
    public float actualSpeed = 0f;
    public float endingSpeed = 5f;
    public float timeToReachMaxSpeed = 10f;

    private float baseEndingSpeed;
    private float baseTimeToReachMaxSpeed;
    private float t = 0.0f;


    private Vector3 basePosition;
    private void Start()
    {
        baseEndingSpeed = endingSpeed;
        baseTimeToReachMaxSpeed = timeToReachMaxSpeed;
        basePosition = this.transform.position;
    }
    void Update()
    {
        if(trigger != null && trigger.GetComponent<PreBossTrigger>().triggered)
        {
            timeToReachMaxSpeed = trigger.GetComponent<PreBossTrigger>().newTimetoReachMaxSpeed;
            endingSpeed = trigger.GetComponent<PreBossTrigger>().newEndingSpeed;
        }

        actualSpeed = Mathf.Lerp(startingSpeed, endingSpeed, t);
        t += (1 / timeToReachMaxSpeed) * Time.deltaTime;

        if (isQuaranteCinqDegre)
            this.transform.position += new Vector3(actualSpeed * Time.deltaTime, actualSpeed * Time.deltaTime, 0);
        else if (isHorizontal && !isQuaranteCinqDegre)
            this.transform.position += new Vector3(actualSpeed * Time.deltaTime, 0, 0);
        else
            this.transform.position += new Vector3(0, actualSpeed * Time.deltaTime, 0);
    }

    public void Reset()
    {
        actualSpeed = startingSpeed;
        endingSpeed = baseEndingSpeed;
        timeToReachMaxSpeed = baseTimeToReachMaxSpeed;
        t = 0.0f;
        this.transform.position = basePosition;
    }
}
