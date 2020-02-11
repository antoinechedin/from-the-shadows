using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SpawnGameManager();
        GameManager.Instance.CurrentSave = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            testClass test = new testClass(new SerializableDate(System.DateTime.Now));
            Debug.Log(JsonUtility.ToJson(test));
        }
    }
}
