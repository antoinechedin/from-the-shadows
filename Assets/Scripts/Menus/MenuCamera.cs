using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public List<GameObject> cameraPositions;
    public List<GameObject> cameraPositionsZoom;
    public ChapterCursor cursor;
    public List<GameObject> cursorPositions;
    public float cameraSpeed;

    public Canvas canvas;

    private int chapterSelected;
    private bool isMoving = false;
    private bool zoom = false;

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            // Move the camera
            Vector3 targetPosition;
            if (!zoom)
            {
                targetPosition = cameraPositions[chapterSelected].transform.position;
            }
            else
            {
                targetPosition = cameraPositionsZoom[chapterSelected].transform.position;
            }
            Vector3 velocity = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.1f);

            // Rotate the camera
            Transform targetRotation = cameraPositions[chapterSelected].transform;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation.rotation, Time.deltaTime * cameraSpeed);

            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, cursorPositions[chapterSelected].transform.position);
            cursor.GetComponent<RectTransform>().anchoredPosition = screenPoint - canvas.GetComponent<RectTransform>().sizeDelta / 2f;

            isMoving = !(velocity.magnitude == 0); // Doesn't actualise the camera position while not moving
        }
    }

    public void SetZoom(bool isZooming)
    {
        zoom = isZooming;
        isMoving = true;
    }

    /// <summary>
    /// Set the current chapter selected.
    /// </summary>
    /// <param name="number">The chapter to select.</param>
    public void SetChapterSelected(int number)
    {
        chapterSelected = number;
        cursor.setPositionNumber(number);
        isMoving = true;
    }
}
