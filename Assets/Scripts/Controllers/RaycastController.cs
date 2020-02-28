using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
    public LayerMask collisionMask = 1 << 9;
    protected const float skinWidth = 0.04f;
    protected const float maxRaySpacing = 0.05f;

    protected int hRayCount;
    protected int vRayCount;
    protected float hRaySpacing;
    protected float vRaySpacing;
    [HideInInspector] public BoxCollider2D boxCollider;
    protected RaycastOrigins raycastOrigins;

    protected virtual void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        InitRaySpacing();
    }

    protected void UpdateRaycastOrigins()
    {
        this.raycastOrigins.bottomLeft = new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.min.y);
        this.raycastOrigins.bottomRight = new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.min.y);
        this.raycastOrigins.topLeft = new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.max.y);
        this.raycastOrigins.topRight = new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.max.y);
    }

    protected void InitRaySpacing()
    {
        float width = boxCollider.bounds.size.x;
        float height = boxCollider.bounds.size.y;

        this.hRayCount = Mathf.FloorToInt(height / maxRaySpacing) + 2;
        this.vRayCount = Mathf.FloorToInt(width / maxRaySpacing) + 2;
        this.hRaySpacing = height / (hRayCount - 1);
        this.vRaySpacing = width / (vRayCount - 1);
    }

    protected struct RaycastOrigins
    {
        public Vector2 bottomLeft, bottomRight, topLeft, topRight;
    }

}
