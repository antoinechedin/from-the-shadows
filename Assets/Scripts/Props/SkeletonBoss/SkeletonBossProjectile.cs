using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBossProjectile : MonoBehaviour
{
    private float speed;
    private Vector3 targetPosition;

    private Vector3 direction;
    void Start()
    {
        speed = this.transform.parent.GetComponent<ProjectileSpawner>().projectileSpeed;
        targetPosition = this.transform.parent.GetComponent<ProjectileSpawner>().target.transform.position;
        targetPosition.y += 1f;

        direction = (targetPosition - this.transform.position ).normalized;

        StartCoroutine(DestroyGameObject());
    }

    IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
