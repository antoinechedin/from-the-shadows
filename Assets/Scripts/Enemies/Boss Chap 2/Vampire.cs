using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : MonoBehaviour, IResetable
{
    [Header("Déplacements")]
    public List<Transform> positions;
    public float speed;

    [Header("Actions")]
    public float minTimeAction;
    public float maxTimeAction;

    public int maxLife;

    private float cptAction;
    private float rdmAction;

    private int life;

    // Start is called before the first frame update
    void Start()
    {
        rdmAction = Random.Range(minTimeAction, maxTimeAction);
    }

    // Update is called once per frame
    void Update()
    {
        cptAction += Time.deltaTime;

        if (cptAction >= rdmAction)
        {
            PickRandomAction();
        }
    }
    public void PickRandomAction()
    {
        int rdm = Random.Range(0, 2); //rdm entre 0 et 1 compris

        switch (rdm)
        {
            case 0:
                Attack();
                break;
            case 1:
                Move();
                break;
            default:
                break;
        }

        cptAction = 0;
        rdmAction = Random.Range(minTimeAction, maxTimeAction);
    }

    public void Attack()
    {
        Debug.Log("Attack");
    }

    public void Move()
    {
        Debug.Log("Move");
        //pick a random pos
        Vector3 chosenPos = positions[Random.Range(0, positions.Count - 1)].position;
        StartCoroutine(MoveAsync(chosenPos));
    }

    public IEnumerator MoveAsync(Vector3 pos)
    {
        while (transform.position != pos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, speed);
            yield return null;
        }
    }

    public void TakeDamage()
    {
        life--;
        Debug.Log("Take damage : " + life);

        if (life <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("DIE");
    }

    public void Reset()
    {
        life = maxLife;
        //TODO : Reset la position
    }
}
