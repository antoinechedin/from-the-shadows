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
    public FileInfo[] saves = new FileInfo[3];

    private Canvas actionChoiceCanvas;
    private Canvas newGameChoiceCanvas;
    private MenuManager menuManager;

    void Start()
    {
        GameManager.Instance.LoadAllSaveFiles();
        GetExistingSaves();
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
        if (Input.GetButtonDown("Return"))
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
    /// Search for existing save files in the saves directory and load them in saves[]
    /// </summary>
    void GetExistingSaves()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
        FileInfo[] filesInfo = directoryInfo.GetFiles();
        foreach (FileInfo f in filesInfo)
        {
            switch (f.Name)
            {
                case "SaveFile0.json":
                    saves[0] = f;
                    break;
                case "SaveFile1.json":
                    saves[1] = f;
                    break;
                case "SaveFile2.json":
                    saves[2] = f;
                    break;
            }
        }
        DisplayInfoOnButtons();
    }

    /// <summary>
    /// Get file infos and link each save to the corresponding button
    /// </summary>
    void DisplayInfoOnButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (saves[i] != null)
            {
                Text empty = buttons[i].transform.Find("Text").GetComponent<Text>();
                empty.text = "";
                Text name = buttons[i].transform.Find("SaveInfo").transform.Find("SaveName").GetComponent<Text>();
                name.text = saves[i].Name;
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
        Text name = buttons[index].transform.Find("SaveInfo").transform.Find("SaveName").GetComponent<Text>();
        name.text = "";
    }

    /// <summary>
    /// Display the canvas ActionChoice when a button is clicked
    /// </summary>
    /// <param name="index"> Index of the button clicked </param>
    public void ChooseAction(int index)
    {
        if (currentState == State.Loading && saves[index] != null)
        {
            actionChoiceCanvas = gameObject.transform.Find("ActionChoice").gameObject.GetComponent<Canvas>();
            Button playButton = actionChoiceCanvas.transform.Find("PlayButton").GetComponent<Button>();
            Button deleteButton = actionChoiceCanvas.transform.Find("DeleteButton").GetComponent<Button>();

            actionChoiceCanvas.gameObject.SetActive(true);
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(playButton.gameObject);

            playButton.onClick.AddListener(delegate { Launch(index); });
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(delegate { Delete(index); });
        }
        else if (saves[index] == null)
        {
            newGameChoiceCanvas = gameObject.transform.Find("NewGameChoice").gameObject.GetComponent<Canvas>();
            Button soloButton = newGameChoiceCanvas.transform.Find("SoloButton").GetComponent<Button>();
            Button duoButton = newGameChoiceCanvas.transform.Find("DuoButton").GetComponent<Button>();

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
        Debug.Log("Load save number " + indexSave);
        actionChoiceCanvas.gameObject.SetActive(false);
        newGameChoiceCanvas.gameObject.SetActive(false);
        menuManager.OpenChaptersMenu();
    }

    /// <summary>
    /// Delete a save file and go back to save selection
    /// </summary>
    /// <param name="indexSave"> Index of the file to delete</param>
    public void Delete(int indexSave)
    {
        GameManager.Instance.DeleteSaveFile(indexSave);
        Debug.Log("Delete save number " + indexSave);

        gameObject.transform.Find("ActionChoice").gameObject.SetActive(false);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(buttons[indexSave].gameObject);

        saves[indexSave] = null;
        GetExistingSaves();
    }

    /// <summary>
    ///  Create a new save file
    /// </summary>
    /// <param name="nbPlayer"> Number of players</param>
    /// <param name="indexSave"> Index of the new file</param>
    public void NewGame(int nbPlayer, int indexSave)
    {
        GameManager.Instance.GetComponent<GameManager>();
        GameManager.Instance.CreateSaveFile(indexSave, nbPlayer);
        GetExistingSaves();
        Debug.Log("Create save number " + indexSave + "( " + nbPlayer + " players )");
        Launch(indexSave);
    }
}
