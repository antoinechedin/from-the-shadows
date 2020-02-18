using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
    void Update()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(GameManager.Instance.Debuging);
        }
    }
}
