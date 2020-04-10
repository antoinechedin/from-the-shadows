using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public enum GUIType { AlwaysDisplayed, DisplayAndHide, DisplayOnce, DisplayAfterTime }

public class OverHeadGUI : MonoBehaviour
{
    private const float timeBetweenCharacter = 0.1f;

    GameObject canvasGO;
    GameObject textGO;
    GameObject imageGO;

    Canvas canvas;
    TextMeshPro text;
    Image image;

    public bool isSoloPlayerSpeaking = false;

    public GUIType type;

    [Header("Place in \"content\" the canvas containing all the UI elements you wish to display")]
    public GameObject content;

    [Space(10)]
    [Tooltip("the number of player needed to display the content")]
    public int nbPlayerNeeded;

    [Header("Time before the player can pass the dialogue box")]
    public float timeBeforePass = 0f;
    public bool canPass = false;

    [Header("Note : set the target to \"this\" to make it static.")]
    [Space(-10)]
    [Header("The GameObject over which the content should be displayed above.")]
    public Transform target;
    public bool faceCamera;
    [Header("(Active only if the type is set to DisplayAfterTime)")]
    public float timeBeforeDisplay;

    private int currentNbPlayer = 0;
    private bool UIActive = false;
    private float timeCount = 0;

    private Animator animator;
    private TextMeshProUGUI textUGUI;
    private string textLine;

    public UnityEvent OnDialogueStart, OnDialogueEnd;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        textUGUI = transform.Find("Content/DialogueBoxBackground/MainText").GetComponent<TextMeshProUGUI>();
        textLine = textUGUI.text;
        canPass = false;
    }


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

    IEnumerator CanPassDialogue()
    {
        yield return new WaitForSeconds(timeBeforePass);
        canPass = true;
    }

    private IEnumerator PrintTextLineCoroutine()
    {
        int i = 1;
        while (i <= textLine.Length)
        {
            textUGUI.text = "<alpha=\"#FF\">" + textLine.Substring(0, i)
                + "<alpha=\"#00\">" + textLine.Substring(i, i - textLine.Length);

            float timeToWait = Char.IsPunctuation(textLine[i - 1]) ? timeBetweenCharacter * 2 : timeBetweenCharacter;
            yield return new WaitForSeconds(timeToWait);
            i++;
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

        if (type == GUIType.DisplayAfterTime)
        {
            timeCount += Time.deltaTime;
            if (timeCount >= timeBeforeDisplay)
            {
                DisplayUI();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            currentNbPlayer++;
        }

        if (currentNbPlayer >= nbPlayerNeeded && type != GUIType.DisplayAfterTime)
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

    public void DisplayUI()
    {
        UIActive = true;
        animator.SetBool("display", true);
        animator.SetBool("hide", false);

        if (isSoloPlayerSpeaking) // Set the right name & image according to the player state in solo mode
        {
            if (FindObjectOfType<CinematicPlayerSwitch>() != null && FindObjectOfType<CinematicPlayerSwitch>().playerState == "Shadow")
            {
                transform.Find("Content/DialogueBoxBackground/SpeakerName").GetComponent<TextMeshProUGUI>().text = "Shadow";
                transform.Find("Content/DialogueBoxBackground/SpeakerImage").GetComponent<Image>().overrideSprite = GetComponent<DialogueBox>().shadowDialogueIcon;
            }
            else
            {
                transform.Find("Content/DialogueBoxBackground/SpeakerName").GetComponent<TextMeshProUGUI>().text = "Light";
                transform.Find("Content/DialogueBoxBackground/SpeakerImage").GetComponent<Image>().overrideSprite = GetComponent<DialogueBox>().lightDialogueIcon;
            }
        }
        StartCoroutine(PrintTextLineCoroutine());
        StartCoroutine(CanPassDialogue());
        //content.SetActive(UIActive);
    }

    public void HideUI()
    {
        UIActive = false;
        animator.SetBool("hide", true);
        animator.SetBool("display", false);
        //content.SetActive(UIActive);
    }

    public virtual void ExecuteOnDialogueStart()
    {
        OnDialogueStart.Invoke();
    }

    public virtual void ExecuteOnDialogueEnd()
    {
        OnDialogueEnd.Invoke();
    }
}

