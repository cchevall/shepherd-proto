using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffLimit : MonoBehaviour
{
    private float minZPos = -50f;
    private float maxZPos = 700f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > maxZPos || transform.position.z < minZPos)
        {
            Destroy(gameObject);
        }
    }
}
