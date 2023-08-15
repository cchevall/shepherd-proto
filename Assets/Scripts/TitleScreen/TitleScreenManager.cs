using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TitleScreenManager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(1);

            //ScoreManager.Instance.AddScore("AAA", Random.Range(0, 42));
            //ScoreManager.Instance.LoadScores();
            //Debug.Log(ScoreManager.Instance.GetScores());
        }
    }
}
