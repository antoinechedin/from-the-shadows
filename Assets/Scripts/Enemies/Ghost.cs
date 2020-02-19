using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float speed;
    public float perceptionRadius;
    public float keepFocusRadius;

    private List<GameObject> players;
    private GameObject target;
    [SerializeField]
    private GhostState state;

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
                break;
            case GhostState.Chase:

                //si la target va trop loin, on perd le focus
                if (Vector3.Distance(transform.position, target.transform.position) > keepFocusRadius)
                {
                    target = null;
                    state = GhostState.Patrol;
                }

                //on se dirige vers la target
                if (target != null)
                {
                    Vector3 dir = target.transform.position - transform.position;
                    dir.Normalize();
                    transform.position += dir * speed * Time.deltaTime;
                }
                break;
            default:
                break;
        }
    }



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, perceptionRadius);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, keepFocusRadius);
    }
#endif
}

public enum GhostState {Patrol, Chase}
