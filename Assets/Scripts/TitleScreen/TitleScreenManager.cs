using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] GameObject rankingScreen;
    [SerializeField] GameObject titleScreen;
    [SerializeField] TextMeshProUGUI[] rankingArray;
    [SerializeField] bool isRankingShown = false;

    private void Start()
    {
        LoadRanking();
    }

    void OnStart()
    {
        SceneManager.LoadScene(1);
    }

    void OnExit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    void OnShowRanking()
    {
        if (GameManager.isLoaded() && !GameManager.Instance.isStarted)
        {
            isRankingShown = !isRankingShown;
            titleScreen.SetActive(!isRankingShown);
            rankingScreen.SetActive(isRankingShown);
        }
    }

    void LoadRanking()
    {
        List<ScoreManager.ScoreEntry> scores = ScoreManager.Instance.GetScores();
        int rankIndex = 0;
        foreach (ScoreManager.ScoreEntry score in scores)
        {
            int curRank = rankIndex + 1;
            if (rankingArray.Length < curRank)
            {
                Debug.LogError("rankingArray length should be equal to ScoreManager _scoreMaxCount");
                break;
            }
            rankingArray[rankIndex].text = $"{curRank} - {score.playerName} {score.score} pts";
            rankIndex++;
        }
    }
}
