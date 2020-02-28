using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScreenshot : MonoBehaviour
{
    private RectTransform rt;
    private Vector3 destination;
    private bool destinationChanged = false;
    private Vector2 startScale;
    public int levelIndex; //the index of the IG level

    private MenuLevels menuLevels;

    public int LevelIndex
    {
        get { return levelIndex; }
        set { levelIndex = value; }
    }

    private void Start()
    {
        menuLevels = GameObject.FindObjectOfType<MenuLevels>();
        rt = GetComponent<RectTransform>();

        startScale = rt.localScale;
        destination = rt.localPosition;
    }
    // Update is called once per frame
    void Update()
    {
        HandleScaling();

        if (destinationChanged)
        {
            StartCoroutine(Move());
            destinationChanged = false;
        }
    }

    public IEnumerator Move()
    {
        while (Vector3.Distance(rt.localPosition, destination) > 0.1f)
        {
            rt.localPosition = Vector3.Lerp(rt.localPosition, destination, menuLevels.speed * Time.deltaTime);
            yield return null;
        }
        rt.localPosition = destination;
    }

    /// <summary>
    /// Sets the new destination to be transform + distance
    /// </summary>
    /// <param name="distance"></param>
    public void SetNewDestination(Vector3 distance)
    {
        destination += distance;
        if (!destinationChanged)
        {
            destinationChanged = true;
        }
    }

    private void HandleScaling()
    {
        Vector3 pos = GetComponent<RectTransform>().localPosition;

        float finalSize = menuLevels.maxSize - (Mathf.Abs(pos.x) / menuLevels.distanceBetweenScreenshots);
        finalSize = Mathf.Clamp(finalSize, menuLevels.minSize, menuLevels.maxSize);
        transform.localScale = startScale * new Vector3(finalSize, finalSize, 1);
    }
}
