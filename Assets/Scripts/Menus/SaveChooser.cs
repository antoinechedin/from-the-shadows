using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class SaveChooser : MonoBehaviour
{
    public enum State { Loading, Saving };
    public State currentState;
    public string directoryPath;
    public Button[] buttons;
    public FileInfo[] saves = new FileInfo[3];


    // Start is called before the first frame update
    void Start()
    {
        GetExistingSaves();
        DisplayInfoOnButtons();
    }

    void GetExistingSaves()
    {
        var directoryInfo = new DirectoryInfo(directoryPath);
        var filesInfo = directoryInfo.GetFiles();
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
    }

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
    
    public void ResetButton(int index)
    {
        Text empty = buttons[index].transform.Find("Text").GetComponent<Text>();
        empty.text = "(vide)";
        Text name = buttons[index].transform.Find("SaveInfo").transform.Find("SaveName").GetComponent<Text>();
        name.text = "";
    }

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

        }
        else if (currentState == State.Saving)
        {

        }
    }

    public void Launch(int indexSave)
    {
        Debug.Log("Load save number "+indexSave);
    }

    public void Delete(int indexSave)
    {
        Debug.Log("Delete save number " + indexSave);
    }        
 
}
