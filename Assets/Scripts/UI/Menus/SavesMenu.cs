using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class SavesMenu : MonoBehaviour, IDissolveMenu
{
    [HideInInspector] public MenuManager menuManager;
    public RectTransform actionChoiceButtons;
    public RectTransform newGameChoiceButtons;

    public Button[] savesButons;

    public AudioClip returnAudioClip;

    private int lastSelected = 0;

    void Start()
    {
        UpdateButtons();
        if (savesButons.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(savesButons[0].gameObject);
        }
    }

    void Update()
    {
        if (EventSystem.current.sendNavigationEvents)
        {
            if (Input.GetButtonDown("B_G"))
            {
                if (actionChoiceButtons != null && actionChoiceButtons.gameObject.activeSelf)
                {
                    StartCoroutine(CloseChoiceButtonsCoroutine(actionChoiceButtons));
                }
                else if (newGameChoiceButtons != null && newGameChoiceButtons.gameObject.activeSelf)
                {
                    StartCoroutine(CloseChoiceButtonsCoroutine(newGameChoiceButtons));
                }
                else
                {
                    // menuManager.OpenStartMenu();
                    menuManager.DissolveFromMenuToMenu(this, menuManager.mainMenu);
                }
                GetComponentInParent<Canvas>().GetComponent<AudioSource>().PlayOneShot(returnAudioClip);
            }
        }
    }

    /// <summary>
    /// Get file infos and link each save to the corresponding button
    /// </summary>
    public void UpdateButtons()
    {
        for (int i = 0; i < savesButons.Length; i++)
        {
            if (GameManager.Instance.Saves[i] != null)
            {
                TextMeshProUGUI buttonText = savesButons[i].transform.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText == null)
                {
                    Debug.LogWarning("WARN SavrMenu.UpdateButtons(): Couldn't find a TMPro Text Component in "
                                     + Utils.GetFullName(savesButons[i].transform)
                    );
                    return;
                }

                string saveName = "Save " + (i + 1).ToString();
                string completion = (int)(GameManager.Instance.Saves[i].GetCompletion() * 100) + "%";
                string timePlayed = TimeSpan.FromSeconds(GameManager.Instance.GetMetaFloat("totalTimePlayed", i)).ToString(@"hh\:mm");

                buttonText.text = saveName + "  " + completion + " " + timePlayed;
            }
            else
            {
                ResetButton(i);
            }
        }
    }

    /// <summary>
    /// Reset a button
    /// </summary>
    /// <param name="index"> Index of the button to reset </param>
    public void ResetButton(int index)
    {
        TextMeshProUGUI buttonText = savesButons[index].transform.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText == null)
        {
            Debug.LogWarning("WARN SavrMenu.ResetButton(): Couldn't find a TMPro Text Component in "
                             + Utils.GetFullName(savesButons[index].transform)
            );
            return;
        }

        buttonText.text = "New Game";
    }

    /// <summary>
    /// Display the canvas ActionChoice when a button is clicked
    /// </summary>
    /// <param name="index"> Index of the button clicked </param>
    public void ChooseAction(int index)
    {
        StartCoroutine(ChooseActionCoroutine(index));
    }

    public IEnumerator ChooseActionCoroutine(int index)
    {
        EventSystem.current.sendNavigationEvents = false;
        lastSelected = index;
        if (GameManager.Instance.Saves[index] != null)
        {
            Button playButton = actionChoiceButtons.transform.Find("PlayButton").GetComponent<Button>();
            Button deleteButton = actionChoiceButtons.transform.Find("DeleteButton").GetComponent<Button>();

            actionChoiceButtons.gameObject.SetActive(true);
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(delegate { Launch(index); });
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(delegate { Delete(index); });
            EventSystem.current.SetSelectedGameObject(playButton.gameObject);

            DissolveController[] dissolves = actionChoiceButtons.GetComponentsInChildren<DissolveController>();
            foreach (DissolveController dissolve in dissolves)
            {
                StartCoroutine(dissolve.DissolveInCoroutine(menuManager.dissolveDuration));
            }
            yield return new WaitForSeconds(menuManager.dissolveDuration);
        }
        else if (GameManager.Instance.Saves[index] == null)
        {
            Button soloButton = newGameChoiceButtons.transform.Find("SoloButton").GetComponent<Button>();
            Button duoButton = newGameChoiceButtons.transform.Find("DuoButton").GetComponent<Button>();

            newGameChoiceButtons.gameObject.SetActive(true);
            soloButton.onClick.RemoveAllListeners();
            soloButton.onClick.AddListener(delegate { NewGame(1, index); });
            duoButton.onClick.RemoveAllListeners();
            duoButton.onClick.AddListener(delegate { NewGame(2, index); });
            EventSystem.current.SetSelectedGameObject(soloButton.gameObject);

            DissolveController[] dissolves = newGameChoiceButtons.GetComponentsInChildren<DissolveController>();
            foreach (DissolveController dissolve in dissolves)
            {
                StartCoroutine(dissolve.DissolveInCoroutine(menuManager.dissolveDuration));
            }
            yield return new WaitForSeconds(menuManager.dissolveDuration);
        }
        EventSystem.current.sendNavigationEvents = true;
    }

    private IEnumerator CloseChoiceButtonsCoroutine(RectTransform choiceButtons)
    {
        EventSystem.current.sendNavigationEvents = false;
        EventSystem.current.SetSelectedGameObject(savesButons[lastSelected].gameObject);

        DissolveController[] dissolves = choiceButtons.GetComponentsInChildren<DissolveController>();
        foreach (DissolveController dissolve in dissolves)
        {
            StartCoroutine(dissolve.DissolveOutCoroutine(menuManager.dissolveDuration));
        }
        yield return new WaitForSeconds(menuManager.dissolveDuration);
        
        choiceButtons.gameObject.SetActive(false);
        EventSystem.current.sendNavigationEvents = true;
    }

    /// <summary>
    /// Launch the game with the selected save file
    /// </summary>
    /// <param name="indexSave"> Index of the file to load</param>
    public void Launch(int indexSave)
    {
        GameManager.Instance.CurrentSave = indexSave;
        // actionChoiceButtons.gameObject.SetActive(false);
        // newGameChoiceButtons.gameObject.SetActive(false);
        List<Chapter> chapters = GameManager.Instance.GetChapters();
        GameManager.Instance.CurrentChapter = 0;
        for (int i = 0; i < chapters.Count - 1; i++)
        {
            if (chapters[i].isCompleted())
            {
                GameManager.Instance.CurrentChapter = i + 1;
            }
        }
        // menuManager.OpenChaptersMenu(GameManager.Instance.CurrentChapter, -1);
        menuManager.DissolveFromMenuToMenu(this, menuManager.chaptersMenu);
    }

    /// <summary>
    /// Delete a save file and go back to save selection
    /// </summary>
    /// <param name="indexSave"> Index of the file to delete</param>
    public void Delete(int indexSave)
    {
        SaveManager.Instance.DeleteSaveFile(indexSave);
        Debug.Log("Delete save number " + indexSave);

        actionChoiceButtons.gameObject.SetActive(false);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(savesButons[indexSave].gameObject);

        GameManager.Instance.Saves[indexSave] = null;
        UpdateButtons();
    }

    /// <summary>
    ///  Create a new save file
    /// </summary>
    /// <param name="nbPlayer"> Number of players</param>
    /// <param name="indexSave"> Index of the new file</param>
    public void NewGame(int nbPlayer, int indexSave)
    {
        GameManager.Instance.GetComponent<GameManager>();
        SaveManager.Instance.CreateSaveFile(indexSave, nbPlayer);
        UpdateButtons();
        Debug.Log("Create save number " + indexSave + " (" + nbPlayer + " players).");
        Launch(indexSave);
    }

    public int LastSelected
    {
        get => lastSelected;
        set => lastSelected = value;
    }

    public IEnumerator DissolveInCoroutine()
    {
        gameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(savesButons[LastSelected].gameObject);
        UpdateButtons();

        DissolveController[] dissolves = GetComponentsInChildren<DissolveController>();
        for (int i = 0; i < dissolves.Length - 1; i++)
        {
            StartCoroutine(dissolves[i].DissolveInCoroutine(menuManager.dissolveDuration));
            yield return new WaitForSeconds(menuManager.dissolveOffset);
        }

        yield return StartCoroutine(dissolves[dissolves.Length - 1].DissolveInCoroutine(menuManager.dissolveDuration));
        EventSystem.current.sendNavigationEvents = true;
    }

    public IEnumerator DissolveOutCoroutine()
    {
        EventSystem.current.sendNavigationEvents = false;

        DissolveController[] dissolves = GetComponentsInChildren<DissolveController>();
        for (int i = 0; i < dissolves.Length - 1; i++)
        {
            StartCoroutine(dissolves[i].DissolveOutCoroutine(menuManager.dissolveDuration));
            yield return new WaitForSeconds(menuManager.dissolveOffset);
        }

        yield return StartCoroutine(dissolves[dissolves.Length - 1].DissolveOutCoroutine(menuManager.dissolveDuration));
        gameObject.SetActive(false);
    }
}
