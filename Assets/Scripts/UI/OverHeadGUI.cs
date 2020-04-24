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
    private const float timeBetweenCharacter = 0.03f;

    GameObject canvasGO;
    GameObject textGO;
    GameObject imageGO;

    Canvas canvas;
    TextMeshPro text;
    Image image;

    public bool isSoloPlayerSpeaking = false;

    public GUIType type;
    public bool isOverhead = false;
    private bool useAnimator;

    [Header("Place in \"content\" the canvas containing all the UI elements you wish to display")]
    public GameObject content;

    [Space(10)]
    [Tooltip("the number of player needed to display the content")]
    public int nbPlayerNeeded;

    [Header("Time before the player can pass the dialogue box")]
    public float animationDuration = 0f;
    [HideInInspector] public bool animationEnded = false;

    [Header("Note : set the target to \"this\" to make it static.")]
    [Space(-10)]
    [Header("The GameObject over which the content should be displayed above.")]
    public Transform target;
    public bool faceCamera;
    [Header("(Active only if the type is set to DisplayAfterTime)")]
    public float timeBeforeDisplay;
    private int currentNbPlayer = 0;
    public bool UIActive = false;
    private float timeCount = 0;

    private AudioSource parentAudioSource;
    private Animator animator;
    private TextMeshProUGUI textUGUI;
    private string textLine;
    [HideInInspector] public bool textLineFullyDisplayed = false;
    private bool skipTextLineAnimation;
    private int textLineIndex;

    public UnityEvent OnDialogueStart, OnDialogueEnd;

    private void Awake()
    {
        parentAudioSource = GetComponentInParent<AudioSource>();
        animator = GetComponent<Animator>();
        if (animator == null) useAnimator = false; else useAnimator = true;
        if (!isOverhead)
        {
            textUGUI = transform.Find("Content/DialogueBoxBackground/MainText").GetComponent<TextMeshProUGUI>();
            textLine = textUGUI.text;
            textUGUI.text = "";
        }
        animationEnded = false;
        textLineFullyDisplayed = false;
        skipTextLineAnimation = false;
        textLineIndex = 0;
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
        yield return new WaitForSeconds(animationDuration);
        animationEnded = true;
    }

    private IEnumerator PrintTextLineCoroutine()
    {
        yield return null;
        textLineIndex = 1;
        while (textLineIndex < textLine.Length)
        {
            if (skipTextLineAnimation) break;

            textUGUI.text = GenerateTMPTextLine(textLine, textLineIndex);
            float timeToWait =
                ".,?!;".Contains(textLine[textLineIndex - 1].ToString()) ?
                timeBetweenCharacter * 7 : timeBetweenCharacter;

            if (textLineIndex % 2 == 0) parentAudioSource.Play();
            yield return new WaitForSeconds(timeToWait);
            textLineIndex++;
        }

        textLineIndex = textLine.Length;
        textUGUI.text = GenerateTMPTextLine(textLine, textLineIndex);
        textLineFullyDisplayed = true;
    }

    private static string GenerateTMPTextLine(string textLine, int i)
    {
        return "<alpha=#FF>" + textLine.Substring(0, i) + "<alpha=#00>" + textLine.Substring(i, textLine.Length - i);
    }

    private void Update()
    {
        if (UIActive && useAnimator)
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

        if (!skipTextLineAnimation && textLineIndex > 0 && InputManager.GetActionPressed(0, InputAction.Jump))
            skipTextLineAnimation = true;
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
        if (useAnimator)
        {
            animator.SetBool("display", true);
            animator.SetBool("hide", false);
        }
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

        if (!isOverhead)
        {
            StartCoroutine(PrintTextLineCoroutine());
            StartCoroutine(CanPassDialogue());
        }
        else
        {
            KeyIndicator[] keyIndicators = GetComponentsInChildren<KeyIndicator>();
            if (keyIndicators != null)
            {
                foreach (var keyIndicator in keyIndicators)
                {
                    keyIndicator.UpdateIndicator();
                }
            }
        }

        content.SetActive(UIActive);
    }

    public void HideUI()
    {
        UIActive = false;
        if (useAnimator)
        {
            animator.SetBool("hide", true);
            animator.SetBool("display", false);
        }
        content.SetActive(UIActive);
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

