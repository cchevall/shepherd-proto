using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float gameSpeed {
        get { return _gameSpeed; }
        set {
            if (value > 0f) {
                _gameSpeed = value;
            }
        }
    }
    public bool isPaused {get; private set;}

    [SerializeField] bool _isPaused = false;
    [SerializeField] float _gameSpeed = 30f;

    public static bool isLoaded()
    {
        if (Instance != null) {
            return true;
        }
        Debug.LogError("No instance of GameManager found.");
        return false;
    }

    public void Pause()
    {
        _isPaused = !_isPaused;
        if (_isPaused) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }
    }

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
