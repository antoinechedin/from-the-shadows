using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireExplosive : MonoBehaviour
{
    public float speed;
    public GameObject projectilePrefab;
    public int nbProjectile;

    private Vector3 targetPos;

    // Update is called once per frame
    void Update()
    {
        if (targetPos != null && transform.position != targetPos)
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        else
            Explode();
    }

    public void Explode()
    {
        Debug.Log("Explode");
        float step = 360 / nbProjectile;

        for (int i = 0; i < nbProjectile; i++)
        {
            Instantiate(projectilePrefab, gameObject.transform.position, Quaternion.Euler(0, 0, i * step));
        }

        Destroy(gameObject);
    }

    public void SetTargetPos(Vector3 pos)
    {
        targetPos = pos;
    }
}
