using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource tickAudio;

    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

    private void OnValidate()
    {
        if (tickAudio == null)
            tickAudio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        WheelEvents.OnSliceTick += PlayTickSound;
    }

    private void OnDisable()
    {
        WheelEvents.OnSliceTick -= PlayTickSound;
    }

    private void PlayTickSound()
    {
        if (tickAudio == null) return;

        tickAudio.Stop();   
        tickAudio.Play();
    }
}