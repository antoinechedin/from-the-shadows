using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public bool finishedFadingIn;
    public bool finished;

    public GameObject loadingLabel;

    private void Update()
    {
        if (finished)
        {
            Destroy(gameObject);
        }
    }
}
