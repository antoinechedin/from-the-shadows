using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPanels : MonoBehaviour
{
    public RectTransform panel1;
    public RectTransform panel2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            panel1.gameObject.SetActive(!panel1.gameObject.activeSelf);
            panel2.gameObject.SetActive(!panel2.gameObject.activeSelf);
        }
    }
}
