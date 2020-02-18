using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class LightSource : MonoBehaviour
{
    public float lightRadius = 3.5f;
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
        circleCollider.radius = 0.5f;
    }

    private void Update()
    {
        transform.localScale = new Vector3(lightRadius * 2, lightRadius * 2, 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("LayeredSolid"))
        {
            other.GetComponent<LayeredObstacle>().AddLightSource(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("DisLayeredSolid"))
        {
            other.GetComponent<LayeredObstacle>().RemoveLightSource(this);
        }
    }
}
