﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePlatform : MonoBehaviour, IResetable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator Destruct()
    {
        gameObject.SetActive(false);
        GameObject.FindObjectOfType<ChapterManager>().StartCameraShake(1f, 1f);

        for (int i = 0; i < 5; i++)
        {
            float rdmX = Random.Range(GetComponent<Collider2D>().bounds.min.x, GetComponent<Collider2D>().bounds.max.x);
            float rdmY = Random.Range(GetComponent<Collider2D>().bounds.min.y, GetComponent<Collider2D>().bounds.max.y);
            Vector3 rdmPos = new Vector3(rdmX, rdmY, gameObject.transform.position.z);
            Instantiate(Resources.Load("DestroyPlatform"), rdmPos, Quaternion.identity);
        }

        yield return new WaitForSeconds(1);
        GameObject.FindObjectOfType<ChapterManager>().StopCameraShake();
    }

    public void Reset()
    {
        gameObject.SetActive(true);
    }
}
