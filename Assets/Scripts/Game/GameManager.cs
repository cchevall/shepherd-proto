using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject spawnManagerPrefab;

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
    [SerializeField] Canvas _userInterfaceCanvas;
    [SerializeField] Canvas _gameOverCanvas;
    [SerializeField] Canvas _pauseCanvas;

    private bool _isGameOver = false;
    private bool _isGameOverCouroutineDone = true;

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
        _isStarted = true;
        if (_userInterfaceCanvas) {
            _userInterfaceCanvas.gameObject.SetActive(true);
        } else {
            Debug.LogError("No User Interface set");
        }
        Instantiate(spawnManagerPrefab, new Vector3(0f, 10f, 800f), spawnManagerPrefab.transform.rotation);
    }

    public void Retry()
    {
        if (!_isGameOverCouroutineDone) {
            return ;
        }
        ResetGame();
        _userInterfaceCanvas.gameObject.SetActive(true);
        SceneManager.LoadScene(1);
    }

    public void GoToTitle()
    {
        if (!_isGameOverCouroutineDone) {
            return ;
        }
        ResetGame();
        SceneManager.LoadScene(0);
    }

    private void ResetGame()
    {
        _gameSpeed = _initialGameSpeed;
        _isStarted = false;
        _isPaused = false;
        _isGameOver = false;
        _gameOverCanvas.gameObject.SetActive(false);
        _pauseCanvas.gameObject.SetActive(false);
        if (_userInterfaceCanvas) {
            _userInterfaceCanvas.gameObject.SetActive(false);
        } else {
            Debug.LogError("No User Interface set");
        }
        Time.timeScale = 1f;
    }

    private IEnumerator EndGame()
    {
        _isGameOver = true;
        _isGameOverCouroutineDone = false;
        _gameSpeed = 0f;
        yield return new WaitForSeconds(2.5f);
        _gameOverCanvas.gameObject.SetActive(true);
        ScoreManager.Instance.AddScoreToBoard();
        _isGameOverCouroutineDone = true;
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
