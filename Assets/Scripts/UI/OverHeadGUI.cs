using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverHeadGUI : MonoBehaviour
{
    GameObject canvas;
    GameObject panel;
    GameObject textGO;
    GameObject imageGO;
    Text text;
    Image image;

    public Transform target;
    public Sprite displayedSprite;
    public string displayedText;
    public Vector3 offSet;

    public bool drawDebugPosition;

    private void Start()
    {
        canvas = GameObject.Find("GUI");

        panel = new GameObject(target.name + " GUI");
        panel.transform.parent = canvas.transform;

        textGO = new GameObject("Text");
        textGO.transform.parent = panel.transform;

        text = textGO.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.alignment = TextAnchor.MiddleCenter;
        text.text = displayedText;

        imageGO = new GameObject("Image");
        imageGO.transform.parent = panel.transform;

        image = imageGO.AddComponent<Image>();
        image.sprite = displayedSprite;
    }

    private void Update()
    {        
        textGO.transform.position = Camera.main.WorldToScreenPoint(target.position + offSet +  offSet);
        imageGO.transform.position = Camera.main.WorldToScreenPoint(target.position + offSet);
    }

    private void OnDrawGizmos()
    {
        if(drawDebugPosition)
            Gizmos.DrawWireSphere(target.position + offSet, 1);
    }
}
