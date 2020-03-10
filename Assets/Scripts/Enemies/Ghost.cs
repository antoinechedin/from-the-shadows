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
                if (checkPoints.Count > 0)
                {
                    if (Vector3.Distance(transform.position, checkPoints[currentCheckPoint]) < 0.1f) //on est arrivé au checkPoint
                    {
                        GetNextCheckPoint();
                    }

                    Vector3 moveDir = checkPoints[currentCheckPoint] - transform.position;
                    moveDir.Normalize();
                    transform.position += moveDir * patrolSpeed * Time.deltaTime;
                }

                ScanForPlayers();

                if (target != null)
                {
                    state = PatrolState.PlayerDetection;
                }
                break;

            case PatrolState.Dead:
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
        transform.localScale = Vector3.zero;
        transform.rotation = Quaternion.identity;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0f;
        animator.SetTrigger("Revive");
        this.enabled = true;
        animator.SetBool("PlayerDetected", false);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        GetComponent<MeshRenderer>().enabled = true;
        base.Reset();
    }

    public void Die(Vector2 from)
    {
        animator.SetTrigger("Die");
        state = PatrolState.Dead;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Rigidbody2D>().AddForce((new Vector2(transform.position.x, transform.position.y) - from).normalized * 200);
    }

    public void AfterDeadNimation()
    {
        Instantiate(Resources.Load("GhostDeath"), transform.position, Quaternion.identity);
        GetComponent<MeshRenderer>().enabled = false;
        this.enabled = false;
    }

    public void SpawnAnimation()
    {
        Instantiate(Resources.Load("GhostDeath"), transform.position + new Vector3(0, 0, -1), Quaternion.identity);
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
