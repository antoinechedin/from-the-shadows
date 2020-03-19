using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Disactivated()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void Activated()
    {
        StartCoroutine(FindObjectOfType<Prism>().SpiningLasers(3));
    }
}
