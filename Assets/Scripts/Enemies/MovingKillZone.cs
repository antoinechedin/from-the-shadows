using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingKillZone : MonoBehaviour
{
    public GameObject killZone;
    [Header("Parent object containing the checkpoints")]
    public GameObject checkPoints;

    private Transform targetPos;
    private float maxSpeed;
    private float timeToMaxSpeed;

    private float currentSpeed = 0;
    private float startSpeed;

    private MovingKillZoneCP currentCheckPoint;

    public Transform TargetPos
    {
        get { return targetPos; }
        set { targetPos = value; }
    }

    public float MaxSpeed
    {
        get { return maxSpeed; }
        set { maxSpeed = value; }
    }

    public float TimeToMaxSpeed
    {
        get { return timeToMaxSpeed; }
        set { maxSpeed = value; }
    }




    // Start is called before the first frame update
    void Start()
    {
        SetNewInfos(checkPoints.transform.GetChild(0).GetComponent<MovingKillZoneCP>());
    }

    // Update is called once per frame
    void Update()
    {
        //on incrément jusqu'à la vitesse max en timeToMaxSpeed secondes
        if (maxSpeed - currentSpeed > 0.1f)
        {
            currentSpeed += Mathf.Lerp(startSpeed, maxSpeed, Time.deltaTime / timeToMaxSpeed);
        }

        //déplacement vers la cible
        killZone.transform.Translate((targetPos.position - transform.position).normalized * currentSpeed * Time.deltaTime);
    }

    public void SetNewInfos(MovingKillZoneCP cp)
    {
        currentCheckPoint = cp;

        this.targetPos = cp.targetPos;
        this.maxSpeed = cp. maxSpeed;
        this.timeToMaxSpeed = cp.timeToMaxSpeed;

        startSpeed = currentSpeed;
    }
}