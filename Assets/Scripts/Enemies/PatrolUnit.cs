using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatrolUnit : MonoBehaviour, IResetable
{
    public bool loopPatrol;
    public float patrolSpeed;
    public float chaseSpeed;
    public float perceptionRadius;
    public float keepFocusRadius;
    public Transform checkPointsParent;

    protected int currentCheckPoint = 0;
    protected int sens = 1; //1 = droite, -1 = gauche
    protected List<Vector3> checkPoints = new List<Vector3>();
    [SerializeField]
    protected PatrolState state;
    protected GameObject target = null;

    private Vector3 initialPos;

    private void Awake()
    {
        initialPos = transform.position;
        //on rempli la liste des checkPoint en fonction des enfants du checkPointsParent
        if (checkPointsParent != null)
        {
            for (int i = 0; i < checkPointsParent.childCount; i++)
            {
                checkPoints.Add(checkPointsParent.GetChild(i).position);
            }
        }
    }


    /// <summary>
    /// returns the next position the Unit should go. It it reaches the limits (whether it is mion or max) the behavior depends on if the 
    /// Unit is set to Looping or not.
    /// </summary>
    public void GetNextCheckPoint()
    {
        if (loopPatrol)
        {
            if (currentCheckPoint + sens > checkPoints.Count - 1)
            {
                currentCheckPoint = 0;
            }
            else if (currentCheckPoint + sens < 0)
            {
                currentCheckPoint = checkPoints.Count - 1;
            }
            else
            {
                currentCheckPoint += sens;
            }

        }
        else
        {
            if (currentCheckPoint + sens > checkPoints.Count - 1 || currentCheckPoint + sens < 0)
            {
                sens *= -1;
            }
            currentCheckPoint += sens;
        }
    }


    public void Reset()
    {
        transform.position = initialPos;
        sens = 1;
        currentCheckPoint = 0;
        state = PatrolState.Patrol;
        target = null;
    }



#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        //radius de perception et focus
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, perceptionRadius);
        UnityEditor.Handles.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, keepFocusRadius);


        //pathing de la patrouille
        if (checkPointsParent != null)
        {
            int cpt = 0;
            for (int i = 0; i < checkPointsParent.childCount - 1; i++)
            {
                Gizmos.DrawLine(checkPointsParent.GetChild(i).position, checkPointsParent.GetChild(i + 1).position);
                UnityEditor.Handles.Label((checkPointsParent.GetChild(i).position + checkPointsParent.GetChild(i + 1).position) / 2, i.ToString());
                cpt++;
            }
            if (loopPatrol && checkPointsParent.childCount > 0)
            {
                Gizmos.DrawLine(checkPointsParent.GetChild(0).position, checkPointsParent.GetChild(checkPointsParent.childCount - 1).position);
                UnityEditor.Handles.Label((checkPointsParent.GetChild(0).position + checkPointsParent.GetChild(checkPointsParent.childCount - 1).position) / 2, cpt.ToString());
            }
        }
    }
    #endif
}

public enum PatrolState { Patrol, Chase, Attack, PlayerDetection, Dead}
