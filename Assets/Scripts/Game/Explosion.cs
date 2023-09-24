using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionParticle;
    [SerializeField] AudioSource spatialAudioSource;
    [SerializeField] List<AudioClip> explosionClips;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayAnimationAndDestroy());
    }

    IEnumerator PlayAnimationAndDestroy()
    {
        explosionParticle.Play();
        AudioClip clip = explosionClips[Random.Range(0, explosionClips.Count)];
        spatialAudioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(6f);
        explosionParticle.Stop();
        Destroy(gameObject);
    }
}
