using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverHeadGUI : MonoBehaviour
{
    GameObject canvasGO;
    GameObject textGO;
    GameObject imageGO;

    Canvas canvas;
    TextMeshPro text;
    Image image;

    public bool drawDebugCanvas = true;
    public Transform target;
    [Header("Content")]
    public Sprite displayedSprite;
    public string displayedText;
    [Header("Position and Size")]
    public Vector3 offSet;
    public Vector3 size = new Vector3(3f, 1f, 0f);
    public bool faceCamera;

    private bool UIActive = true;

    private void Start()
    {
        CreateCanvas();
        AddText();
        if (displayedSprite == null)
            Debug.LogWarning("Can't find sprite to display over " + gameObject.name);
        else
            AddImage();
        ToggleUI();
    }

    private void Update()
    {
        if (UIActive)
        {
            canvasGO.transform.position = target.position + offSet;
            if (faceCamera)
                canvasGO.transform.LookAt(canvasGO.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            ToggleUI();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            ToggleUI();
    }

    private void ToggleUI()
    {
        UIActive = !UIActive;
        canvasGO.SetActive(UIActive);
    }

    private void CreateCanvas()
    {
        canvasGO = new GameObject("GUI Canvas");
        canvasGO.transform.parent = transform;
        canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();
        canvasGO.GetComponent<RectTransform>().sizeDelta = size;
    }

    private void AddImage()
    {
        imageGO = new GameObject("Image");
        imageGO.transform.parent = canvasGO.transform;
        image = imageGO.AddComponent<Image>();
        image.sprite = displayedSprite;
        image.useSpriteMesh = true;
        image.preserveAspect = true;
        RectTransform imageRT = imageGO.GetComponent<RectTransform>();
        imageRT.anchorMin = Vector2.zero;
        imageRT.anchorMax = new Vector2(1f, 0.6f);
        imageRT.sizeDelta = Vector2.zero;
        imageRT.position += new Vector3(0, 0, 0.7f);
    }

    private void AddText()
    {
        textGO = new GameObject("Text");
        textGO.transform.parent = canvasGO.transform;
        text = textGO.AddComponent<TextMeshPro>();
        text.font = TMP_FontAsset.CreateFontAsset(Resources.GetBuiltinResource<Font>("Arial.ttf"));
        text.enableAutoSizing = true;
        text.fontSizeMin = 0;
        text.fontSizeMax = 50;
        text.enableWordWrapping = false;
        text.alignment = TextAlignmentOptions.Center;
        text.text = displayedText;
        RectTransform textRT = textGO.GetComponent<RectTransform>();
        textRT.anchorMin = new Vector2(0, 0.6f);
        textRT.anchorMax = Vector2.one;
        textRT.sizeDelta = Vector2.zero;
        textRT.position += new Vector3(0, 0, 0.7f);
    }

    private void OnDrawGizmos()
    {
        if (drawDebugCanvas)
        {
            if (target == null)
            {
                Debug.LogError("Can't find target for " + gameObject.name);
                drawDebugCanvas = false;
                return;
            }
            Gizmos.DrawWireCube(target.position + offSet, size);
        }
    }
}
