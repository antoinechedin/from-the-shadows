using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SpawnGameManager();
        GameManager.Instance.LoadAllSaveFiles();
        GameManager.Instance.CurrentSave = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            List<Level> levels = new List<Level>();
            levels.Add(new Level(false, 1, new int[] { 0 }));
            levels.Add(new Level(false, 1, new int[] { 0 }));

            List<Chapter> chaps = new List<Chapter>();
            chaps.Add(new Chapter(levels));

            Dictionary<string, float> testDic = new Dictionary<string, float>();
            testDic.Add("mabite", 17f);

            Save save = new Save(chaps, 1, new Dictionary<string, int>(), testDic, new System.DateTime());

            Save.TestWriteSaveFile(save);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            Save.TestLoadSaveFile();
        }
    }
}
