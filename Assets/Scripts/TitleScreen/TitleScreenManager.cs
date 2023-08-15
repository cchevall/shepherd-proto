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

    private Coroutine showRankingCoroutine;

    private void Start()
    {
        LoadRanking();
        showRankingCoroutine = StartCoroutine(ShowRanking());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (isRankingShown)
            {
                HideRanking();
            }
            else
            {
                StopCoroutine(showRankingCoroutine);
                SceneManager.LoadScene(1);
            }
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

    void HideRanking()
    {
        isRankingShown = false;
        rankingScreen.SetActive(false);
        titleScreen.SetActive(true);
        StartCoroutine(ShowRanking());
    }

    IEnumerator ShowRanking()
    {
        yield return new WaitForSeconds(5);
        isRankingShown = true;
        titleScreen.SetActive(false);
        rankingScreen.SetActive(true);
    }
}
