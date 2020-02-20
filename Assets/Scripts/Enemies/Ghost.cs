using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : PatrolUnit
{
    private GameObject target = null;

    // Start is called before the first frame update
    void Start()
    {
        state = PatrolState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case PatrolState.Patrol:
                //mouvement de vas et vient
                if (Vector3.Distance(transform.position, checkPoints[currentCheckPoint]) < 0.1f) //on est arrivé au checkPoint
                {
                    GetNextCheckPoint();
                }

                Vector3 moveDir = checkPoints[currentCheckPoint] - transform.position;
                moveDir.Normalize();
                transform.position += moveDir * patrolSpeed * Time.deltaTime;

                //détection de cible
                List<GameObject> possibleTargets = new List<GameObject>();

                //TODO : vers le overlapCircle que sur le layer Player quand il sera mis en place
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, perceptionRadius);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].CompareTag("Player"))
                    {
                        possibleTargets.Add(colliders[i].gameObject);
                    }
                }

                foreach (GameObject go in possibleTargets)
                {
                    if (target == null || Vector3.Distance(transform.position, go.transform.position) < Vector3.Distance(transform.position, target.transform.position))
                    {
                        target = go;
                    }
                }

                if (target != null)
                {
                    state = PatrolState.Chase;
                }
                break;




            case PatrolState.Chase:
                //si la target va trop loin, on perd le focus
                if (target == null || Vector3.Distance(transform.position, target.transform.position) > keepFocusRadius)
                {
                    target = null;
                    state = PatrolState.Patrol;
                }

                //on se dirige vers la target
                if (target != null)
                {
                    Vector3 dir = target.transform.position - transform.position;
                    dir.Normalize();
                    transform.position += dir * chaseSpeed * Time.deltaTime;
                }
                break;





            default:
                Debug.LogWarning(name + " : PatrolState not set.");
                break;
        }
    }
}
