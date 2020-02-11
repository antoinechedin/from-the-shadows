using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float timeBeforeDestroy;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeBeforeDestroy);
    }
}
