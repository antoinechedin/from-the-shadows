using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public bool finishedFadingIn;
    public bool finished;

    private void Update()
    {
        if (finished)
        {
            Destroy(gameObject);
        }
    }
}
