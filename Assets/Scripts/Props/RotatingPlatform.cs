using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : ActivatorListener
{
    public float angleRotation = 90f;
    public float speedRotation = 5;

    private Quaternion startRotation;
    private Quaternion targetRotation;


    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.rotation;
        targetRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * speedRotation);
    }

    public override void OnActivate()
    {
        Quaternion rot = Quaternion.AngleAxis(angleRotation, new Vector3(0, 0, 1));
        targetRotation = targetRotation * rot;
    }

    public override void OnDeactivate()
    {
        targetRotation = startRotation;
    }
}
