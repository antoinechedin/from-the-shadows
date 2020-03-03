using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public Transform cameraTarget;

    void Start()
    {
        if (cameraTarget == null)
        {
            GameObject defaultCameraTarget = new GameObject("Camera Target");
            defaultCameraTarget.gameObject.AddComponent<CameraTarget>();
            cameraTarget = defaultCameraTarget.transform;
            Debug.Log("Create a default Camera Target");
        }
    }

    public void ProcessCameraConfiner(BoxCollider2D limits, CinemachineVirtualCamera virtualCamera, BoxCollider newCollider, float maxCamDepth)
    {
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

            newCollider.transform.position = new Vector3((leftBottom.x + topRight.x) / 2f, cameraHeight + leftBottom.y + vSize / 2f, -maxDepth);
            newCollider.size = new Vector3(topRight.x - leftBottom.x - hOffset, vSize, 0);
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

            newCollider.transform.position = new Vector3((leftBottom.x + topRight.x) / 2f, leftBottom.y + vOffset, -maxDepth);
            newCollider.size = new Vector3(hSize, vSize, 0);
        }

        virtualCamera.Follow = cameraTarget;
        virtualCamera.m_Follow = cameraTarget;
        virtualCamera.gameObject.GetComponent<CinemachineConfiner>().m_BoundingVolume = newCollider;
    }
}
