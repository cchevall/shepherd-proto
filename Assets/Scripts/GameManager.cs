using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] float _initialGameSpeed = 30f;
    [SerializeField] float _gameSpeed = 30f;
    [SerializeField] Canvas _userInterfaceCanvas;
    [SerializeField] Canvas _gameOverCanvas;
    [SerializeField] Canvas _pauseCanvas;

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
        StartCoroutine(EndGame());
    }

    public void Pause()
    {
        _isPaused = !_isPaused;
        if (_isPaused) {
            Time.timeScale = 0f;
            _pauseCanvas.gameObject.SetActive(true);
        } else {
            Time.timeScale = 1f;
            _pauseCanvas.gameObject.SetActive(false);
        }
    }

    public void InitGame()
    {
        if (_userInterfaceCanvas) {
            _userInterfaceCanvas.gameObject.SetActive(true);
        } else {
            Debug.LogError("No User Interface set");
        }
    }

    public void Retry()
    {
        ResetGame();
        _userInterfaceCanvas.gameObject.SetActive(true);
        SceneManager.LoadScene(1);
    }

    public void GoToTitle()
    {
        ResetGame();
        _userInterfaceCanvas.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }

    private void ResetGame()
    {
        _gameSpeed = _initialGameSpeed;
        _isPaused = false;
        _isGameOver = false;
        _gameOverCanvas.gameObject.SetActive(false);
        _pauseCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private IEnumerator EndGame()
    {
        _gameSpeed = 0f;
        _isGameOver = true;
        yield return new WaitForSeconds(2.5f);
        _gameOverCanvas.gameObject.SetActive(true);
        ScoreManager.Instance.AddScoreToBoard();
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
