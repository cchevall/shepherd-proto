using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource gameMusicAudioSource;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip healthPickupClip;
    [SerializeField] AudioClip pauseClip;
    [SerializeField] AudioClip resumeClip;

    public void PlayHealthPickupSound()
    {
        audioSource.PlayOneShot(healthPickupClip);
    }

    public void PlayPauseSound()
    {
        gameMusicAudioSource.Pause();
        audioSource.PlayOneShot(pauseClip);
    }

    public void PlayResumeSound()
    {
        gameMusicAudioSource.Play();
        audioSource.PlayOneShot(resumeClip);
    }
}
