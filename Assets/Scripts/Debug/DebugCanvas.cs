using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
    private bool active = false;

    void Update()
    {
        if (GameManager.Instance.Debuging != active)
        {
            active = GameManager.Instance.Debuging;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(active);
            }
        }
    }
}
