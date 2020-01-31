using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterCursor : MonoBehaviour
{
    public List<Vector3> positions;

    public void setPositionNumber(int number)
    {
        // GetComponent<RectTransform>().anchoredPosition = positions[number];
    }
}
