using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    /* Code from CodeMonkey's video "How to do Camera Shake with Cinemachine!
     * 
     * 
     * 
     */

    public static CinemachineShake Instance { get; private set;}

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;

    [SerializeField]
    private Transform mousePosition;



    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        ShakeCamera(0.1f, 0.1f);
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                // Timer over!
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 
                    Mathf.Lerp(startingIntensity, 0f, (1 - (shakeTimer / shakeTimerTotal)));
            }
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    public void ShakeCameraAimed(float intensity, float time)
    {

    }
}
