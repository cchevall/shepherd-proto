using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffLimit : MonoBehaviour
{
    private float minZPos = -LevelConfig.offLimitZPos * 2f;
    private float maxZPos = LevelConfig.offLimitZPos * 2f;

    void Update()
    {
        if (transform.position.z > maxZPos || transform.position.z < minZPos || transform.position.y < LevelConfig.offLimitYBottomPos || transform.position.y > LevelConfig.offLimitYTopPos)
        {
            Destroy(gameObject);
        }
    }
}
