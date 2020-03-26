using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class LevelScreenshot : MonoBehaviour, ISelectHandler
{
    public GameObject collectiblesHolder;

    private RectTransform rt;
    [HideInInspector] public Vector3 destination;
    // private bool destinationChanged = false; // inutile
    private Vector2 startScale;
    /// <summary>
    /// The ingame ID of the level
    /// </summary>
    private int levelId;

    /// <summary>
    /// The gameobject index of the screenshots list inside MenuLevel
    /// </summary>
    [HideInInspector] public int levelIndex;

    public Image foreground;

    [HideInInspector]
    public MenuLevels menuLevels;
    private bool pressed = false;
    private AudioSource audioSource;

    private CanvasGroup canvasGroup;

    public int LevelId
    {
        get { return levelId; }
        set { levelId = value; }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        canvasGroup = GetComponent<CanvasGroup>();

        menuLevels = GameObject.FindObjectOfType<MenuLevels>();
        rt = GetComponent<RectTransform>();

        startScale = rt.localScale;
        destination = rt.localPosition;

        HandleAppearence();
    }
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(rt.localPosition, destination) > 0.1f)
        {
            rt.localPosition = Vector3.Lerp(rt.localPosition, destination, menuLevels.speed);
            if (!pressed)
            {
                HandleAppearence();
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

    private void HandleAppearence()
    {
        Vector3 pos = GetComponent<RectTransform>().localPosition;

        float finalSize = menuLevels.maxSize - Mathf.Abs(pos.x) * (menuLevels.maxSize - menuLevels.minSize) / menuLevels.distanceBetweenScreenshots;
        finalSize = Mathf.Clamp(finalSize, menuLevels.minSize, menuLevels.maxSize);
        transform.localScale = startScale * new Vector3(finalSize, finalSize, 1);

        float foregroungAlpha = Mathf.Abs(pos.x) * menuLevels.foregroundMaxAlpha / menuLevels.distanceBetweenScreenshots;
        foregroungAlpha = Mathf.Clamp(foregroungAlpha, 0, menuLevels.foregroundMaxAlpha);
        foreground.color = new Color(0, 0, 0, foregroungAlpha);

        float overallAlpha = 2 - Mathf.Abs(pos.x) / menuLevels.distanceBetweenScreenshots;
        overallAlpha = Mathf.Clamp(overallAlpha, 0f, 1f);
        canvasGroup.alpha = overallAlpha;
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
        if (menuLevels != null)
        {
            menuLevels.SelectCheckpoint(levelIndex);
        }
    }
}
