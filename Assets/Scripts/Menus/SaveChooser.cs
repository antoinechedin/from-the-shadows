using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

public class SaveChooser : MonoBehaviour
{
    public string directoryPath;
    public Button button1;
    public Button button2;
    public Button button3;
    FileInfo save1;
    FileInfo save2;
    FileInfo save3;


    // Start is called before the first frame update
    void Start()
    {
        var directoryInfo = new DirectoryInfo(directoryPath);
        var filesInfo = directoryInfo.GetFiles();
        foreach (FileInfo f in filesInfo)
        {
            switch (f.Name)
            {
                case "1.json":
                    save1 = f;
                    GetInfos(button1, save1);
                    break;
                case "2.json":
                    Debug.Log("allo");
                    save2 = f;
                    GetInfos(button2, save2);
                    break;
                case "3.json":
                    save3 = f;
                    GetInfos(button3, save3);
                    break;
            }
        }
    }

    void GetInfos(Button button, FileInfo file)
    {
        //Text text = button.GetComponentInChildren(typeof(Text)) as Text;
        Text empty = gameObject.transform.Find("Empty").GetComponent<Text>();
        empty.text = "";
        Text name = gameObject.transform.Find("SaveName").GetComponent<Text>();
        empty.text = file.Name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
