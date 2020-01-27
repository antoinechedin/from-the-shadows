using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public List<GameObject> cameraPositions;
    public ChapterCursor cursor;
    public float cameraSpeed;

    private int chapterSelected;
    private bool isMoving = false;

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            // Move the camera
            Vector3 targetPosition = cameraPositions[chapterSelected].transform.position;
            Vector3 velocity = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.1f);

            // Rotate the camera
            Transform targetRotation = cameraPositions[chapterSelected].transform;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation.rotation, Time.deltaTime * cameraSpeed);

            cursor.gameObject.SetActive(velocity.magnitude < 1); // Doesn't show cursor while moving
            isMoving = !(velocity.magnitude == 0); // Doesn't actualise the camera position while not moving
        }
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
