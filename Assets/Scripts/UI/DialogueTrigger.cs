using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("-Set every OverHeadGUIs to \"DisplayAndHide\"")]
    public List<OverHeadGUI> guis;

    private int currentDisplayed = 0;
    private bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        //on les désactive tous au départ
        foreach (OverHeadGUI gUI in guis)
        {
            gUI.HideUI();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("A_G"))
            {
                if (currentDisplayed < guis.Count - 1) //Si c'est pas le dernier, on passe au texte suivant
                {
                    guis[currentDisplayed].HideUI();
                    currentDisplayed++;
                    guis[currentDisplayed].DisplayUI();
                }
                else //Sinon le dialogue est terminé
                {
                    //on cache la dernière boîte de dialogue
                    guis[currentDisplayed].HideUI();

                    //on active les inputs des joueurs
                    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                    foreach (GameObject p in players)
                    {
                        p.GetComponent<PlayerInput>().active = true;
                    }

                    //on détruit la boîte de dialogue
                    Destroy(gameObject, 1f);
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !started)
        {
            //on désactive les inputs des joueurs
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                p.GetComponent<PlayerInput>().active = false;
            }

            //On démarre le dialogue
            guis[0].DisplayUI();
            started = true;
        }
    }
}
