using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayeredObstacle : MonoBehaviour
{
    public LayeredObstacleType type;
    [HideInInspector] public List<LightSource> lightSources;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        lightSources = new List<LightSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
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

        if (spriteRenderer != null)
        {
            Color newColor = spriteRenderer.color;
            if (gameObject.layer == LayerMask.NameToLayer("Obstacle")) newColor.a = 1f;
            if (gameObject.layer == LayerMask.NameToLayer("TransparentObstacle")) newColor.a = 0.1f;
            spriteRenderer.color = newColor;
        }
    }
}

public enum LayeredObstacleType
{
    Light, Shadow
}
