using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class LevelScreenshot : MonoBehaviour, ISelectHandler
{
    public GameObject collectiblesHolder;

    private RectTransform rt;
    private Vector3 destination;
    // private bool destinationChanged = false; // inutile
    private Vector2 startScale;
    private int levelIndex; //the index of the IG level

    [HideInInspector]
    public MenuLevels menuLevels;
    private bool pressed = false;
    private AudioSource audioSource;

    public int LevelIndex
    {
        get { return levelIndex; }
        set { levelIndex = value; }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        menuLevels = GameObject.FindObjectOfType<MenuLevels>();
        rt = GetComponent<RectTransform>();

        startScale = rt.localScale;
        destination = rt.localPosition;

        HandleScaling();
    }
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(rt.localPosition, destination) > 0.1f)
        {
            rt.localPosition = Vector3.Lerp(rt.localPosition, destination, menuLevels.speed);
            if (!pressed)
            {
                HandleScaling();
            }
        }
        else
        {
            // destinationChanged = false;
        }
    }

    /// <summary>
    /// Sets the new destination to be transform + distance
    /// </summary>
    /// <param name="distance"></param>
    public void SetNewDestination(Vector3 distance)
    {
        destination += distance;
        // destinationChanged = true;
    }

    private void HandleScaling()
    {
        Vector3 pos = GetComponent<RectTransform>().localPosition;

        float finalSize = menuLevels.maxSize - (Mathf.Abs(pos.x) / menuLevels.distanceBetweenScreenshots * menuLevels.minSize);
        finalSize = Mathf.Clamp(finalSize, menuLevels.minSize, menuLevels.maxSize);
        transform.localScale = startScale * new Vector3(finalSize, finalSize, 1);
    }

    public IEnumerator PressedAnimation()
    {
        audioSource.Play();
        pressed = true;

        float timeCount = 0;
        while (timeCount < 0.06f)
        {
            transform.localScale *= 1.03f;
            timeCount += Time.deltaTime;
            yield return null;
        }

        timeCount = 0;
        while (timeCount < 0.06f)
        {
            transform.localScale /= 1.03f;
            timeCount += Time.deltaTime;
            yield return null;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(transform.name + " selected");
    }
}
