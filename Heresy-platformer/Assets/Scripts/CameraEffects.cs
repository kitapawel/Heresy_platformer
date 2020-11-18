using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CameraEffects : MonoBehaviour
{
    public float ShakeAmplitude = 2f;
    public float ShakeFrequency = 2f;

    private static float ShakeElapsedTime = 0f;

    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    private void Start()
    {
        if (virtualCamera != null)
        {
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        // If the Cinemachine componet is not set, avoid update
        if (virtualCamera != null && virtualCameraNoise != null)
        {
            // If Camera Shake effect is still playing
            if (ShakeElapsedTime > 0)
            {
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

                // Update Shake Timer
                ShakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                // If Camera Shake effect is over, reset variables
                virtualCameraNoise.m_AmplitudeGain = 0f;
                ShakeElapsedTime = 0f;
            }
        }
    }
    public static void ScreenShakeAtHit()
    {
        ShakeElapsedTime = 0.2f;
    }
}