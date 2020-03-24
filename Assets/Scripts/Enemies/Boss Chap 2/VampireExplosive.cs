using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireExplosive : MonoBehaviour
{
    public GameObject projectilePrefab;

    private float speed;
    private int minNbProjectible;
    private int maxNbProjectile;

    private float speedSubProjectile;
    private float lengthSubProjectile;

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
        //off aléatoire de rotation en Z
        float rdmRot = Random.Range(0, 360);

        //choisi aléatoirement le nombre de projectiles
        int rdm = Random.Range(minNbProjectible, maxNbProjectile + 1);

        float step = 360 / rdm;

        for (int i = 0; i < rdm; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, gameObject.transform.position, Quaternion.Euler(0, 0, i * step + rdmRot));
            projectile.GetComponent<LaserProjectile>().SetInfo(speedSubProjectile, lengthSubProjectile);
        }

        Destroy(gameObject);
    }

    public void SetInfos(Vector3 pos, float speedProjectile, int minSubProj, int maxSubProj, float speedSubProj, float lengthSubProj)
    {
        targetPos = pos;
        speed = speedProjectile;
        minNbProjectible = minSubProj;
        maxNbProjectile = maxSubProj;

        speedSubProjectile = speedSubProj;
        lengthSubProjectile = lengthSubProj;
    }
}
