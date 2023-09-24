using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip healthPickupClip;

    public void PlayHealthPickupSound()
    {
        audioSource.PlayOneShot(healthPickupClip);
    }
}