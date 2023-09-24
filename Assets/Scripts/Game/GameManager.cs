using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject spawnManagerPrefab;
    SoundManager soundManager;

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

    public bool isStarted {
        get {
            return _isStarted;
        }
    }

    public bool isGameOver {
        get {
            return _isGameOver;
        }
    }

    [SerializeField] bool _isStarted = false;
    [SerializeField] bool _isPaused = false;
    [SerializeField] float _initialGameSpeed = 45f;
    [SerializeField] float _gameSpeed = 45f;

    private bool _isGameOver = false;

    public static bool isLoaded()
    {
        if (Instance == null) {
            Debug.LogError("No instance of GameManager found.");
            return false;
        }
        return true;
    }

    public void BoostGameSpeed(float multiplier)
    {
        if (multiplier < 1f) {
            return ;
        }
        _gameSpeed = _initialGameSpeed * multiplier;
    }

    public void GameOver()
    {
        _isGameOver = true;
        _gameSpeed = 0f;
        ScoreManager.Instance.AddScoreToBoard();
    }

    public void Pause()
    {
        _isPaused = !_isPaused;
        if (_isPaused) {
            Time.timeScale = 0f;
            soundManager.PlayPauseSound();
        } else {
            Time.timeScale = 1f;
            soundManager.PlayResumeSound();
        }
    }

    public void InitGame()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        _isStarted = true;
        Instantiate(spawnManagerPrefab, new Vector3(0f, 10f, 800f), spawnManagerPrefab.transform.rotation);
    }

    public void Retry()
    {
        ResetGame();
        SceneManager.LoadScene(1);
    }

    public void GoToTitle()
    {
        ResetGame();
        SceneManager.LoadScene(0);
    }

    private void ResetGame()
    {
        _gameSpeed = _initialGameSpeed;
        _isStarted = false;
        _isPaused = false;
        _isGameOver = false;
        Time.timeScale = 1f;
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
