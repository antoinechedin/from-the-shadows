using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMetadata : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.LoadAllSaveFiles();
        GameManager.Instance.CurrentSave = 0;
    }
}
