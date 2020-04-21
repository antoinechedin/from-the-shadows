using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public float amplitude = 1f;
    public float frequency = 1f;

    public void ShakeFor(float time)
    {
        StartCoroutine(ShakeForAsync(amplitude, frequency, time));
    }

    public IEnumerator ShakeForAsync(float amplitude, float frequency, float time)
    {
        StartCameraShake(amplitude, frequency);
        yield return new WaitForSeconds(time);
        StopCameraShake();
    }

    public void StartCameraShake(float amplitude, float frequency)
    {
        this.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        this.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
    }

    public void StopCameraShake()
    {
        this.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
    }

}
