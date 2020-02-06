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
            GameManager.Instance.TestLoadSave(0);
        }
    }
}
