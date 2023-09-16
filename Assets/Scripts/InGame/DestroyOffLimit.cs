using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffLimit : MonoBehaviour
{
    private float minZPos = -50f;
    private float maxZPos = LevelConfig.offLimitZPos + 100f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > maxZPos || transform.position.z < minZPos)
        {
            Destroy(gameObject);
        }
    }
}
