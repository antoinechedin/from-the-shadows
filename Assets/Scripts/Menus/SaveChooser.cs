using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

public class SaveChooser : MonoBehaviour
{
    public string directoryPath;
    public Button button0;
    public Button button1;
    public Button button2;
    FileInfo save0;
    FileInfo save1;
    FileInfo save2;


    // Start is called before the first frame update
    void Start()
    {
        UpdateButtons();
    }

    void UpdateButtons()
    {
        var directoryInfo = new DirectoryInfo(directoryPath);
        var filesInfo = directoryInfo.GetFiles();
        foreach (FileInfo f in filesInfo)
        {
            switch (f.Name)
            {
                case "SaveFile0.json":
                    save0 = f;
                    GetInfos(button0, save0);
                    break;
                case "SaveFile1.json":
                    save1 = f;
                    GetInfos(button1, save1);
                    break;
                case "SaveFile2.json":
                    save2 = f;
                    GetInfos(button2, save2);
                    break;
            }
        }
    }

    void GetInfos(Button button, FileInfo file)
    {
        Text empty = button.transform.Find("Text").GetComponent<Text>();
        empty.text = "";
        Text name = button.transform.Find("SaveInfo").transform.Find("SaveName").GetComponent<Text>();
        name.text = file.Name;
    }

}
