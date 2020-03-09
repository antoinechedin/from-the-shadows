using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Ghost : PatrolUnit, IResetable
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        state = PatrolState.Patrol;
        animator = GetComponent<Animator>();
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

                ScanForPlayers();

                if (target != null)
                {
                    state = PatrolState.PlayerDetection;
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


            case PatrolState.PlayerDetection:
                animator.SetBool("PlayerDetected", true);
                break;


            default:
                Debug.LogWarning(name + " : PatrolState not set.");
                break;
        }
    }

    public new void Reset()
    {
        animator.SetBool("Die", false);
        animator.SetBool("PlayerDetected", false);
        base.Reset();
    }

    public void Die(Vector2 from)
    {
        //TODO : remplacer pour un effet de mort
        animator.SetBool("Die", true);
        this.enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Rigidbody2D>().AddForce(-from * 30);
        Destroy(gameObject, 0.5f);
    }

    /// <summary>
    /// Scans around for players and put the closest (if any) in the target variable.
    /// </summary>
    public void ScanForPlayers()
    {
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
    }

    #region AnimatorFunctions   
    public void SetChaseState()
    {
        ScanForPlayers();

        if (target != null)
        {
            state = PatrolState.Chase;
        }
        else
        {
            state = PatrolState.Patrol;
        }
        animator.SetBool("PlayerDetected", false);
    }
    #endregion

}
