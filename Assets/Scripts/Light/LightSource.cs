using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class LightSource : MonoBehaviour
{
    public float lightRadius = 5f;
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
    }

    private void Update()
    {
        circleCollider.radius = lightRadius;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "LayeredObstacle")
        {
            other.GetComponent<LayeredObstacle>().AddLightSource(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "LayeredObstacle")
        {
            other.GetComponent<LayeredObstacle>().RemoveLightSource(this);
        }
    }
}
