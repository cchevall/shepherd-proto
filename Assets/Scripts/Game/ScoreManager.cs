using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public TextMeshProUGUI scoreText  {
        get {
            return _scoreText;
        }
        set {
            _scoreText = value;
            _scoreText.SetText(ScoreManager.Instance.currentScore.ToString());
        }
    }
    public int currentScore
    {
        get { return _currentScore; }
    }
    public string currentPlayerName {
        get { return _currentPlayerName; }
        set {
            if (IsValidPlayerName(value))
            {
                _currentPlayerName = value;
            } else
            {
                Debug.LogError("Player name should be at least 1 letter long");
            }
        }
    }

    private int _currentScore = 0;
    private string _currentPlayerName = "player";
    private string _scoresFilePath;
    private List<ScoreEntry> _scoreList = new List<ScoreEntry>();
    private int _scoreMaxCount = 10;
    private int _playerNameMaxLength = 11;
    private TextMeshProUGUI _scoreText;

    void Awake()
    {
        if (Instance == null)
        {
            _scoresFilePath = Path.Combine(Application.persistentDataPath, "scores.json");
            DontDestroyOnLoad(gameObject);
            LoadScores();
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    public static bool isLoaded()
    {
        if (Instance != null) {
            return true;
        }
        Debug.LogError("No instance of ScoreManager found.");
        return false;
    }

    public void AddOrRemovePointsToScore(int points)
    {
        _currentScore += points;
        scoreText.SetText(_currentScore.ToString());
    }

    public bool IsValidPlayerName(string name)
    {
        return name.Length > 0 && name.Length < _playerNameMaxLength;
    }

    public bool IsValidScore(int scoreValue)
    {
        return scoreValue >= 0;
    }

    /**
     * Add score to board if high enough to be ranked
     */
    public void AddScoreToBoard()
    {
        if (!IsValidScore(currentScore)) {
            Debug.LogError("Player score should be strictly positive");
            return ;
        }
        if (!IsValidPlayerName(currentPlayerName)) 
        {
            Debug.LogError("Player name should be at least 1 letter long");
            return ;
        }
        ScoreEntry newScore = new ScoreEntry(currentPlayerName, currentScore);
        _scoreList.Add(newScore);
        // Sort List Desc
        _scoreList.Sort((a, b) => b.score.CompareTo(a.score));

        // Remove additionnal scores from list according to scoreMaxLength
        if (_scoreList.Count > _scoreMaxCount)
        {
            _scoreList.RemoveRange(_scoreMaxCount, _scoreList.Count - _scoreMaxCount);
        }
        SaveScores();
        _currentPlayerName = "player";
        _currentScore = 0;
        scoreText.SetText(_currentScore.ToString());
    }

    public bool IsHighScore(int score)
    {
        if (_scoreList.Count == 0){
            return true;
        }
        return _scoreList[0].score <= score;
    }

    /**
     * Gets Current scores list from cache
     * Beware to use LoadScores method before retrieving it on first run
     */
    public List<ScoreEntry> GetScores()
    {
        return _scoreList;
    }

    /**
     * Loads Score List from a json file in persistent data folder
     */
    public void LoadScores()
    {
        if (File.Exists(_scoresFilePath))
        {
            string json = File.ReadAllText(_scoresFilePath);
            _scoreList = JsonUtility.FromJson<ScoreEntryList>(json).scores;
        }
    }

    /**
     * Put Score List to a json file in persistent data folder
     */
    private void SaveScores()
    {
        ScoreEntryList scoreEntryList = new ScoreEntryList(_scoreList);
        string json = JsonUtility.ToJson(scoreEntryList);
        File.WriteAllText(_scoresFilePath, json);
    }

    /**
     * Serializable Score Entry
     */
    [System.Serializable]
    public class ScoreEntry
    {
        public string playerName;
        public int score;

        public ScoreEntry(string name, int score)
        {
            playerName = name;
            this.score = score;
        }
    }

    /**
     * Serializable Score Entry List
     */
    [System.Serializable]
    private class ScoreEntryList
    {
        public List<ScoreEntry> scores;

        public ScoreEntryList(List<ScoreEntry> scores)
        {
            this.scores = scores;
        }
    }
}
