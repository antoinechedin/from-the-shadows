using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public float projectileCooldown = 5f;
    public float projectileSpeed = 2f;

    public GameObject projectile;

    public GameObject target;

    private GameObject[] players = new GameObject[2];

    void Start()
    {
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Player").Length; i++)
        {
            players[i] = GameObject.FindGameObjectsWithTag("Player")[i];
        }

        StartCoroutine(SpawnProjectileLoop());
    }

    IEnumerator SpawnProjectileLoop()
    {
        yield return new WaitForSeconds(projectileCooldown);

        Instantiate(projectile, this.transform.position, Quaternion.identity, this.transform);
        StartCoroutine(SpawnProjectileLoop());
    }

    private void Update()
    {
        if (Vector3.Distance(this.transform.position, players[0].transform.position) >= Vector3.Distance(this.transform.position, players[1].transform.position))
            target = players[1];
        else
            target = players[0];
    }
}
