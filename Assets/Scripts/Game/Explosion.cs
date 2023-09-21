using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionParticle;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayAnimationAndDestroy());
    }

    IEnumerator PlayAnimationAndDestroy()
    {
        explosionParticle.Play();
        yield return new WaitForSeconds(6f);
        explosionParticle.Stop();
        Destroy(gameObject);
    }
}
