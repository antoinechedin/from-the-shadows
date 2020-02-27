using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTarget : MonoBehaviour
{
    public BoxCollider2D limits;
    public CinemachineVirtualCamera virtualCamera;
    public BoxCollider boxCollider;
    public Vector2 offset;

    private GameObject[] players = new GameObject[2];

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        transform.position = (players[0].transform.position + players[1].transform.position) / 2 + (Vector3)offset;
    }

    void FixedUpdate()
    {
        transform.position = (players[0].transform.position + players[1].transform.position) / 2 + (Vector3)offset;
    }

    void ProcessCameraConfiner()
    {
        float maxCamDepth = 35;
        float frameAspectRatio = (float)Screen.width / (float)Screen.height;
        float cameraDistance = Mathf.Abs(virtualCamera.transform.position.z);
        float frameHeight = cameraDistance * 2f * Mathf.Tan(virtualCamera.m_Lens.FieldOfView * Mathf.Deg2Rad / 2f);
        float frameWidth = frameHeight * frameAspectRatio;
        float limitWidth = limits.size.x;
        float limitHeight = limits.size.y;
        float limitAspectRatio = limitWidth / limitHeight;
        float vfov = virtualCamera.m_Lens.FieldOfView;
        float hfov = (2f * Mathf.Atan(Mathf.Tan(vfov * Mathf.Deg2Rad / 2f) * frameAspectRatio)) * Mathf.Rad2Deg;
        float teta = virtualCamera.transform.rotation.eulerAngles.x;
        Vector3 leftBottom = limits.transform.TransformPoint(limits.offset + limits.size * new Vector2(-0.5f, -0.5f));
        Vector3 topRight = limits.transform.TransformPoint(limits.offset + limits.size * new Vector2(0.5f, 0.5f));

        if (limitAspectRatio > frameAspectRatio)
        {
            float maxDepth = limitHeight / (Mathf.Tan((vfov / 2f + teta) * Mathf.Deg2Rad) + Mathf.Tan((vfov / 2f - teta) * Mathf.Deg2Rad));
            float cameraHeight = 0;
            float vSize = 0;
            float hOffset = frameAspectRatio * limitHeight;

            if (maxDepth > maxCamDepth)
            {
                cameraHeight = maxCamDepth * Mathf.Tan((vfov / 2f + teta) * Mathf.Deg2Rad);
                vSize = limitHeight * (maxDepth - maxCamDepth) / maxDepth;
                hOffset = frameAspectRatio * limitHeight * maxCamDepth / maxDepth;
                maxDepth = maxCamDepth;
            }
            else
            {
                cameraHeight = maxDepth * Mathf.Tan((vfov / 2f + teta) * Mathf.Deg2Rad);
            }

            boxCollider.transform.position = new Vector3((leftBottom.x + topRight.x) / 2f, cameraHeight + leftBottom.y + vSize / 2f, -maxDepth);
            boxCollider.size = new Vector3(topRight.x - leftBottom.x - hOffset, vSize, 0);
        }
        else
        {
            float maxDepth = limitWidth / (2f * (Mathf.Tan((hfov / 2f) * Mathf.Deg2Rad)));
            float hSize = 0;

            if (maxDepth > maxCamDepth)
            {
                hSize = limitWidth * (maxDepth - maxCamDepth) / maxDepth;
                maxDepth = maxCamDepth;
            }

            float verticalPadding = maxDepth * (Mathf.Tan((vfov / 2f + teta) * Mathf.Deg2Rad) + Mathf.Tan((vfov / 2f - teta) * Mathf.Deg2Rad));
            float bottomPadding = maxDepth * Mathf.Tan((vfov / 2f + teta) * Mathf.Deg2Rad);
            float vSize = topRight.y - leftBottom.y - verticalPadding;
            float vOffset = vSize / 2f + bottomPadding;

            boxCollider.transform.position = new Vector3((leftBottom.x + topRight.x) / 2f, leftBottom.y + vOffset, -maxDepth);
            boxCollider.size = new Vector3(hSize, vSize, 0);
        }
    }

    public void SetCameraLimits(BoxCollider2D newLimits, CinemachineVirtualCamera newVirtualCamera, BoxCollider newCollider)
    {
        limits = newLimits;
        boxCollider = newCollider;
        virtualCamera = newVirtualCamera;
        virtualCamera.Follow = transform;
        virtualCamera.gameObject.GetComponent<CinemachineConfiner>().m_BoundingVolume = newCollider;
        ProcessCameraConfiner();
    }
}
