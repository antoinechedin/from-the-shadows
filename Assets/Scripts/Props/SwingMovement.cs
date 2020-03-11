using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingMovement : MonoBehaviour, IResetable
{
    [Range(0,90)]
    public float angle;
    public float speed;
    public float offsetTime;
    public float ropeLength;
    public Transform origin;
    public Transform swingingObject;

    private float currentTime;
    private Quaternion start;
    private Quaternion end;

    // Start is called before the first frame update
    void Start()
    {
        start = RotationPoint(angle);
        end = RotationPoint(-angle);
        currentTime = offsetTime;

        Transform child = swingingObject.transform.Find("Child").transform;
        child.Translate(new Vector3(0, -ropeLength, 0));
        if (swingingObject.GetComponentInChildren<Torch>() != null) swingingObject.GetComponentInChildren<Torch>().neverStatic = true;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        swingingObject.transform.rotation = Quaternion.Lerp(start, end, (Mathf.Sin( currentTime * speed + Mathf.PI / 2) + 1f) / 2f);
    }

    Quaternion RotationPoint(float angle)
    {
        Quaternion rotation = transform.rotation;
        float angleZ = rotation.eulerAngles.z + angle;

        if (angleZ > 180)
            angleZ -= 360;
        else if (angleZ < -180)
            angleZ += 360;

        rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, angleZ);
        return rotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.3f);
        Gizmos.color = Color.yellow;
        Vector3 direction0 = Quaternion.AngleAxis(0, Vector3.zero) * new Vector3(0, -ropeLength, 0);
        Gizmos.DrawRay(transform.position, direction0);

        Vector3 direction1 = Quaternion.AngleAxis(-angle, origin.transform.forward) * -origin.transform.up * ropeLength;
        Gizmos.DrawRay(transform.position, direction1);
        Vector3 direction2 = Quaternion.AngleAxis(angle, origin.transform.forward) * -origin.transform.up * ropeLength;
        Gizmos.DrawRay(transform.position, direction2);
    }

    public void Reset()
    {
        currentTime = offsetTime;
    }
}
