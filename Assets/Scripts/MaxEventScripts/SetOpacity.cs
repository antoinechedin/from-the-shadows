using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOpacity : MonoBehaviour
{
    public float startingOpacity;
    public float endingOpacity;

    public float timeToReachEndingOpacity;

    public float materialOpacity;

    public bool playAtStart = false;

    private float t;
    private bool started = false;

    private Color newColor;
    private Color startingColor;

    void Start()
    {
        startingColor = GetComponent<MeshRenderer>().material.GetColor("_BaseColor");
        materialOpacity = startingOpacity;

        if (playAtStart)
            StartOpacityChange();
    }

    // Update is called once per frame
    void Update()
    {
        if(started)
        {
            t += Time.deltaTime / timeToReachEndingOpacity;
            materialOpacity = Mathf.Lerp(startingOpacity, endingOpacity, t);

            newColor = new Color(startingColor.r, startingColor.g, startingColor.b, materialOpacity);

            GetComponent<MeshRenderer>().material.SetColor("_BaseColor", newColor);
        }

    }

    public void StartOpacityChange()
    {
        started = true;
    }
}
