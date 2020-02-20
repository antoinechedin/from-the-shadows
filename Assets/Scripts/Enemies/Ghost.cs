using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public int nbCheckPoint;
    public bool loopPatrol;
    public float patrolSpeed;
    public float chaseSpeed;
    public float perceptionRadius;
    public float keepFocusRadius;

    [SerializeField]
    private Vector3[] checkPoints;
    private List<GameObject> players;
    private GameObject target = null;
    [SerializeField]
    private GhostState state;
    private int currentCheckPoint = 0;
    private int sens = 1; //1 = droite, -1 = gauche

    private int oldNbCheckPoints;

    // Start is called before the first frame update
    void Start()
    {
        state = GhostState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GhostState.Patrol:
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
                    state = GhostState.Chase;
                }
                break;
            case GhostState.Chase:
                //si la target va trop loin, on perd le focus
                if (target == null || Vector3.Distance(transform.position, target.transform.position) > keepFocusRadius)
                {
                    target = null;
                    state = GhostState.Patrol;
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
                break;
        }
    }

    public void GetNextCheckPoint()
    {
        if (loopPatrol)
        {
            if (currentCheckPoint + sens > checkPoints.Length - 1)
            {
                currentCheckPoint = 0;
            }
            else
            {
                currentCheckPoint += sens;
            }

        }
        else
        {
            if (currentCheckPoint + sens > checkPoints.Length - 1 || currentCheckPoint + sens < 0)
            {
                sens *= -1;
            }
            currentCheckPoint += sens;
        }
    }



    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, perceptionRadius);
        UnityEditor.Handles.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, keepFocusRadius);
    }

    private void OnDrawGizmosSelected()
    {
        int cpt = 0;
        for (int i = 0; i < checkPoints.Length -1; i++)
        {
            Gizmos.DrawLine(checkPoints[i], checkPoints[i+1]);
            UnityEditor.Handles.Label((checkPoints[i] + checkPoints[i + 1]) / 2, i.ToString());
            cpt++;
        }
        if (loopPatrol)
        {
            Gizmos.DrawLine(checkPoints[0], checkPoints[checkPoints.Length - 1]);
            UnityEditor.Handles.Label((checkPoints[0] + checkPoints[checkPoints.Length - 1]) / 2, cpt.ToString());
        }
    }
#endif

    #region Editor
    public void GoToCheckPoint(int i)
    {
        transform.position = checkPoints[i];
    }

    public void SetCheckPoint(int i)
    {
        checkPoints[i] = transform.position;
    }

    public void InitCheckPoints()
    {
        checkPoints = new Vector3[nbCheckPoint];
    }

    public void OnValidate()
    {
        if (!Application.isPlaying  && PlayerPrefs.GetInt("oldNbCheckPoint", 0) != nbCheckPoint)
        {
            PlayerPrefs.SetInt("oldNbCheckPoint", nbCheckPoint);
            InitCheckPoints();
        }

    }
    #endregion



}

public enum GhostState {Patrol, Chase}
