using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    public ParticleSystem psLeft;
    public ParticleSystem psRight;

    public void PlayFootstepParticles(float right)
    { 
        if ((right == 0.1f || right == 1.1f || Random.Range(0, 2) == 0) && Time.time > 2)
        {
            if (right > 0.5) psRight.Play();
            else psLeft.Play();
        }
    }
}
