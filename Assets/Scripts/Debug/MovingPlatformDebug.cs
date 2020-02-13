using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformDebug : MonoBehaviour
{
    GameObject startingPoint;
    GameObject endingPoint;

    public bool drawDebug;

    public void ToggleDrawDebug()
    {
        drawDebug = !drawDebug;
        Transform tmp;
        tmp = transform.Find("_Control Points");
        if(tmp != null)
        {
            for (int i = 0; i < tmp.childCount; i++)
            {
                var mr = tmp.GetChild(i).gameObject.GetComponent<MeshRenderer>();
                mr.enabled = !mr.enabled;
            }
        }
        tmp = transform.Find("Starting Point");
        if (tmp != null)
        {
            var mr = tmp.gameObject.GetComponent<MeshRenderer>();
            mr.enabled = !mr.enabled;
        }
        tmp = transform.Find("Ending Point");
        if (tmp != null)
        {
            var mr = tmp.gameObject.GetComponent<MeshRenderer>();
            mr.enabled = !mr.enabled;
        }
    }

    private void OnDrawGizmos()
    {
        if (drawDebug)
        {
            Transform tmp;

            tmp = transform.Find("Starting Point");
            if (tmp != null)
                startingPoint = tmp.gameObject;

            tmp = transform.Find("Ending Point");
            if (tmp != null)
                endingPoint = tmp.gameObject;

            Gizmos.color = Color.red;

            tmp = transform.Find("_Control Points");

            if (tmp != null)
            {
                Gizmos.DrawLine(startingPoint.transform.position, tmp.GetChild(0).position);
                for (int i = 0; i < tmp.childCount - 1; i++)
                {
                    Transform go1 = tmp.GetChild(i);
                    Transform go2 = tmp.GetChild(i + 1);

                    Gizmos.DrawLine(go1.position, go2.position);
                }
                Gizmos.DrawLine(tmp.GetChild(tmp.childCount - 1).position, endingPoint.transform.position);
            }
            else
            {
                if (startingPoint != null && endingPoint != null)
                    Gizmos.DrawLine(startingPoint.transform.position, endingPoint.transform.position);
            }
        }
    }
}
