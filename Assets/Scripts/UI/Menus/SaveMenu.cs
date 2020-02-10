using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    public enum State { Loading, Saving };
    public State currentState;
    public string directoryPath;
    public Button[] buttons;
    private Save[] saves;

    private Canvas actionChoiceCanvas;
    private Canvas newGameChoiceCanvas;
    private MenuManager menuManager;

    void Start()
    {
        saves = GameManager.Instance.Saves;
        UpdateButtons();
        actionChoiceCanvas = gameObject.transform.Find("ActionChoice").gameObject.GetComponent<Canvas>();
        newGameChoiceCanvas = gameObject.transform.Find("NewGameChoice").gameObject.GetComponent<Canvas>();
        menuManager = GameObject.Find("MenuManager").gameObject.GetComponent<MenuManager>();
        if (buttons.Length > 0)
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(buttons[0].gameObject);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("B_G"))
        {
            if (actionChoiceCanvas.gameObject.activeSelf)
            {
                actionChoiceCanvas.gameObject.SetActive(false);
                EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(buttons[0].gameObject);
            }
            else if (newGameChoiceCanvas.gameObject.activeSelf)
            {
                newGameChoiceCanvas.gameObject.SetActive(false);
                EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(buttons[0].gameObject);
            }
            else
            {
                menuManager.OpenStartMenu();
            }
        }
    }

    /// <summary>
    /// Get file infos and link each save to the corresponding button
    /// </summary>
    void UpdateButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (saves[i] != null)
            {
                Text empty = buttons[i].transform.Find("Text").GetComponent<Text>();
                empty.text = "";
                buttons[i].transform.Find("SaveInfo").gameObject.SetActive(true);
                Text date = buttons[i].transform.Find("SaveInfo").transform.Find("Date").GetComponent<Text>();
                date.text = saves[i].LastOpenDate.ToString();
                Text timePlayed = buttons[i].transform.Find("SaveInfo").transform.Find("TimePlayed").GetComponent<Text>();
                timePlayed.text = GameManager.Instance.GetMetaFloat("totalTimePlayed", i).ToString();
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
        Text empty = buttons[index].transform.Find("Text").GetComponent<Text>();
        empty.text = "NEW GAME";
        buttons[index].transform.Find("SaveInfo").gameObject.SetActive(false);

        Text timePlayed = buttons[index].transform.Find("SaveInfo").transform.Find("TimePlayed").GetComponent<Text>();
        timePlayed.text = "";
        Text date = buttons[index].transform.Find("SaveInfo").transform.Find("Date").GetComponent<Text>();
        date.text = "";
    }

    /// <summary>
    /// Display the canvas ActionChoice when a button is clicked
    /// </summary>
    /// <param name="index"> Index of the button clicked </param>
    public void ChooseAction(int index)
    {
        Button playButton = actionChoiceCanvas.transform.Find("PlayButton").GetComponent<Button>();
        Button deleteButton = actionChoiceCanvas.transform.Find("DeleteButton").GetComponent<Button>();
        Button soloButton = newGameChoiceCanvas.transform.Find("SoloButton").GetComponent<Button>();
        Button duoButton = newGameChoiceCanvas.transform.Find("DuoButton").GetComponent<Button>();

        playButton.onClick.RemoveAllListeners();
        deleteButton.onClick.RemoveAllListeners();
        soloButton.onClick.RemoveAllListeners();
        duoButton.onClick.RemoveAllListeners();

        if (currentState == State.Loading && saves[index] != null)
        {
            actionChoiceCanvas = gameObject.transform.Find("ActionChoice").gameObject.GetComponent<Canvas>();


            actionChoiceCanvas.gameObject.SetActive(true);
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(playButton.gameObject);

            playButton.onClick.AddListener(delegate { Launch(index); });
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(delegate { Delete(index); });
        }
        else if (saves[index] == null)
        {
            newGameChoiceCanvas = gameObject.transform.Find("NewGameChoice").gameObject.GetComponent<Canvas>();


            newGameChoiceCanvas.gameObject.SetActive(true);
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(duoButton.gameObject);

            soloButton.onClick.AddListener(delegate { NewGame(1, index); });
            duoButton.onClick.AddListener(delegate { NewGame(2, index); });
        }
    }

    /// <summary>
    /// Launch the game with the selected save file
    /// </summary>
    /// <param name="indexSave"> Index of the file to load</param>
    public void Launch(int indexSave)
    {
        GameManager.Instance.CurrentSave = indexSave;
        actionChoiceCanvas.gameObject.SetActive(false);
        newGameChoiceCanvas.gameObject.SetActive(false);
        List<Chapter> chapters = GameManager.Instance.GetChapters();
        GameManager.Instance.CurrentChapter = 0;
        for (int i = 0; i < chapters.Count - 1; i++)
        {
            if (chapters[i].isCompleted())
            {
                GameManager.Instance.CurrentChapter = i + 1;
            }
        }
        menuManager.OpenChaptersMenu(GameManager.Instance.CurrentChapter);
    }

    /// <summary>
    /// Delete a save file and go back to save selection
    /// </summary>
    /// <param name="indexSave"> Index of the file to delete</param>
    public void Delete(int indexSave)
    {
        SaveManager.Instance.DeleteSaveFile(indexSave);
        Debug.Log("Delete save number " + indexSave);

        gameObject.transform.Find("ActionChoice").gameObject.SetActive(false);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(buttons[indexSave].gameObject);

        saves[indexSave] = null;
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
}
