using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAttack : MonoBehaviour
{
    public GameObject prefab;
    public float speed = 15;
    public float timeBetweenAttacks = 5;
    public Transform limitLeft;
    public Transform limitRight;
    public Transform player1;
    public Transform player2;

    private GameObject go;
    private Vector3 from;
    private Vector3 to;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Attack", 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (go != null)
            go.transform.position = Vector3.MoveTowards(go.transform.position, to, Time.deltaTime * speed);
        if (go != null && go.transform.position == to)
        {
            Destroy(go);
            go = null;
            Invoke("Attack", timeBetweenAttacks);
        }
    }

    public void Attack()
    {
        FindTarget();

        if (from != null && to != null && prefab != null)
            go = Instantiate(prefab, from, Quaternion.identity);
    }

    public void FindTarget()
    {
        float distLeft1 = Vector3.Distance(player1.position, limitLeft.position);
        float distLeft2 = Vector3.Distance(player2.position, limitLeft.position);
        float distRight1 = Vector3.Distance(player1.position, limitRight.position);
        float distRight2 = Vector3.Distance(player2.position, limitRight.position);

        float min = Mathf.Min(distLeft1, distLeft2, distRight1, distRight2);

        if (min == distLeft1)
        {
            from = new Vector3(limitLeft.position.x, player1.position.y, 0);
            to = new Vector3(limitRight.position.x, player1.position.y, 0);
        }
        else if (min == distLeft2)
        {
            from = new Vector3(limitLeft.position.x, player2.position.y, 0);
            to = new Vector3(limitRight.position.x, player2.position.y, 0);
        }
        else if (min == distRight1)
        {
            from = new Vector3(limitRight.position.x, player1.position.y, 0);
            to = new Vector3(limitLeft.position.x, player1.position.y, 0);
        }
        else if (min == distRight2)
        {
            from = new Vector3(limitRight.position.x, player2.position.y, 0);
            to = new Vector3(limitLeft.position.x, player2.position.y, 0);
        }
    }
}
