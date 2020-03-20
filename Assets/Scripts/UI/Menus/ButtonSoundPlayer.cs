using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundPlayer : MonoBehaviour
{
    public bool disableOnce;

    void PlaySound(AudioClip whichSound)
    {
        if (!disableOnce)
        {
            GetComponentInParent<Canvas>().GetComponent<AudioSource>().PlayOneShot(whichSound);
        }
        else
        {
            disableOnce = false;
        }
    }
}
