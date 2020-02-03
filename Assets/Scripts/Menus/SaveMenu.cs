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


    void Start()
    {
        GetExistingSaves();
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
        for(int i=0; i < buttons.Length; i++)
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
            Canvas actionChoiceCanvas = gameObject.transform.Find("ActionChoice").gameObject.GetComponent<Canvas>();
            Button playButton = actionChoiceCanvas.transform.Find("PlayButton").GetComponent<Button>();
            Button deleteButton = actionChoiceCanvas.transform.Find("DeleteButton").GetComponent<Button>();

            actionChoiceCanvas.gameObject.SetActive(true);
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(playButton.gameObject);

            playButton.onClick.AddListener(delegate { Launch(index); });
            deleteButton.onClick.AddListener(delegate { Delete(index); });
        } else if (saves[index] == null)
        {
            Canvas newGameChoiceCanvas = gameObject.transform.Find("NewGameChoice").gameObject.GetComponent<Canvas>();
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
        GameObject.Find("GameManager").GetComponent<GameManager>().LoadSaveFile(indexSave);
        Debug.Log("Load save number "+indexSave);
        //TODO Afficher le menu de choix des chapitres
    }

    /// <summary>
    /// Delete a save file and go back to save selection
    /// </summary>
    /// <param name="indexSave"> Index of the file to delete</param>
    public void Delete(int indexSave)
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().DeleteSaveFile(indexSave);
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
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.CreateSaveFile(indexSave, nbPlayer);
        Debug.Log("Create save number " + indexSave + "( "+ nbPlayer +" players )");
    } 
}
