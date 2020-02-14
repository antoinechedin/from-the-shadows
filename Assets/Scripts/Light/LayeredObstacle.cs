using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayeredObstacle : MonoBehaviour
{
    public LayeredObstacleType type;
    [HideInInspector] public List<LightSource> lightSources;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        lightSources = new List<LightSource>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start() {
        if (meshRenderer != null)
            meshRenderer.enabled = false;

        UpdateCollider();
    }

    public void AddLightSource(LightSource lightSource)
    {
        lightSources.Add(lightSource);
        UpdateCollider();
    }

    public void RemoveLightSource(LightSource lightSource)
    {
        lightSources.Remove(lightSource);
        UpdateCollider();
    }

    private void UpdateCollider()
    {
        switch (type)
        {
            case LayeredObstacleType.Light:
                if (lightSources.Count == 0)
                {
                    gameObject.layer = LayerMask.NameToLayer("TransparentObstacle");
                }
                else gameObject.layer = LayerMask.NameToLayer("Obstacle");
                break;

            case LayeredObstacleType.Shadow:
                if (lightSources.Count == 0)
                {
                    gameObject.layer = LayerMask.NameToLayer("Obstacle");
                }
                else gameObject.layer = LayerMask.NameToLayer("TransparentObstacle");
                break;
        }

        if (meshRenderer != null)
        {
            if (gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                meshRenderer.enabled = true;
            else if (gameObject.layer == LayerMask.NameToLayer("TransparentObstacle"))
                meshRenderer.enabled = false;
        }
    }
}

public enum LayeredObstacleType
{
    Light, Shadow
}
