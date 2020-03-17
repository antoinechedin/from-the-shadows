using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsatingLight : MonoBehaviour
{
    public int startingIntensity = 12000;
    public int endingIntensity = 6000;

    public float speed = 2f;

    public bool playAtStart = false;

    private bool mustPulse = false;

    private Light _light;
    private float t = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        _light = this.GetComponent<Light>();
        if (playAtStart)
            mustPulse = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (mustPulse)
        {
            t += speed * Time.deltaTime;

            _light.intensity = Mathf.Lerp(startingIntensity, endingIntensity, t);
        }
    }
}
