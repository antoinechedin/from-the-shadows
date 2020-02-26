using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GUIType { AlwaysDisplayed, DisplayAndHide, DisplayOnce }

public class OverHeadGUI : MonoBehaviour
{
    GameObject canvasGO;
    GameObject textGO;
    GameObject imageGO;

    Canvas canvas;
    TextMeshPro text;
    Image image;

    public float test;
    public GUIType type;

    [Header("Place in \"content\" the canvas containing all the UI elements you wish to display")]
    public GameObject content;

    [Space(10)]
    [Tooltip("the number of player needed to display the content")]
    public int nbPlayerNeeded;

    [Header("Note : set the target to \"this\" to make it static.")]
    [Space(-10)]
    [Header("The GameObject over which the content should be displayed above.")]
    public Transform target;
    public bool faceCamera;

    private int currentNbPlayer = 0;
    private bool UIActive = false;


    private void Start()
    {
        if (type == GUIType.AlwaysDisplayed)
        {
            DisplayUI();
        }
        else
        {
            HideUI();
        }
    }

    private void Update()
    {
        if (UIActive)
        {
            content.transform.position = target.position;
            if (faceCamera)
                content.transform.LookAt(content.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            currentNbPlayer++;
        }

        if (currentNbPlayer >= nbPlayerNeeded)
        {
            DisplayUI();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            currentNbPlayer--;
        }

        if (type == GUIType.DisplayAndHide && currentNbPlayer < nbPlayerNeeded)
        {
            HideUI();
        }
    }

    private void DisplayUI()
    {
        UIActive = true;
        content.SetActive(UIActive);
    }

    private void HideUI()
    {
        UIActive = false;
        content.SetActive(UIActive);
    }
}

