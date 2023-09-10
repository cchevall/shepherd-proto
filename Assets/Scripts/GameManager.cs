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
    public bool isPaused {
        get {
            return _isPaused;
        }
    }

    public bool isGameOver {
        get {
            return _isGameOver;
        }
    }

    [SerializeField] bool _isPaused = false;
    [SerializeField] float _gameSpeed = 30f;
    [SerializeField] Canvas _userInterface;
    [SerializeField] Canvas _gameOver;

    private bool _isGameOver = false;

    public static bool isLoaded()
    {
        if (Instance != null) {
            return true;
        }
        Debug.LogError("No instance of GameManager found.");
        return false;
    }

    public void GameOver()
    {
        Pause();
        _isGameOver = true;
        _gameOver.gameObject.SetActive(true);
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

    public void InitGame()
    {
        if (_userInterface) {
            _userInterface.gameObject.SetActive(true);
        } else {
            Debug.LogError("No User Interface set");
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
