using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentGameObject : MonoBehaviour
{
    public static PermanentGameObject Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }
}
