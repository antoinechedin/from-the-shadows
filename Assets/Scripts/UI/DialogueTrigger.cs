using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    public bool mustDisableInput = true;

    [Header("-Set every OverHeadGUIs to \"DisplayAndHide\"")]
    public List<OverHeadGUI> guis;

    public float delay = 0f;

    private int currentDisplayed = 0;
    private bool started = false;
    private bool canPassDialogue = false;

    // Start is called before the first frame update
    void Start()
    {
        //on les désactive tous au départ
        foreach (OverHeadGUI gUI in guis)
        {
            gUI.HideUI();
        }
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(delay);
        guis[0].DisplayUI();

        guis[0].ExecuteOnDialogueStart(); // Execute function at start
        started = true;
        GameManager.Instance.IsInCutscene = true;
    }

    public void StartDialogue()
    {
        //on désactive les inputs des joueurs
        if (mustDisableInput)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                p.GetComponent<PlayerInput>().active = false;
                p.GetComponent<PlayerInput>().noJump = false;
            }
        }

        StartCoroutine(StartDelay());
        //On démarre le dialogue
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            //if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("A_G"))
            if (InputManager.GetActionPressed(0, InputAction.Jump))
            {
                if (guis[currentDisplayed].animationEnded && guis[currentDisplayed].textLineFullyDisplayed)
                {
                    if (currentDisplayed < guis.Count - 1) //Si c'est pas le dernier, on passe au texte suivant
                    {
                        if(guis[currentDisplayed].UIActive)
                            guis[currentDisplayed].ExecuteOnDialogueEnd(); // Execute OnDialogueEnd functions
                        guis[currentDisplayed].HideUI();
                        guis[currentDisplayed].animationEnded = false;

                        currentDisplayed++;

                        guis[currentDisplayed].DisplayUI();
                        guis[currentDisplayed].ExecuteOnDialogueStart(); // Execute OnDialogueStart functions

                    }
                    else //Sinon le dialogue est terminé
                    {
                        //on cache la dernière boîte de dialogue
                        if (guis[currentDisplayed].UIActive)
                            guis[currentDisplayed].ExecuteOnDialogueEnd(); // Execute OnDialogueEnd functions
                        guis[currentDisplayed].HideUI();


                        //on active les inputs des joueurs
                        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                        foreach (GameObject p in players)
                        {
                            p.GetComponent<PlayerInput>().active = true;
                        }
                        //on détruit la boîte de dialogue
                        Destroy(gameObject, 1f);
                        GameManager.Instance.IsInCutscene = false;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !started)
        {
            StartDialogue();
        }
    }
}
