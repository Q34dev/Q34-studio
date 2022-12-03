using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    private MapManager mapManager;

    [SerializeField] private TextMeshProUGUI scoreText, highScoreText;
    [SerializeField] private Transform backgroundParent;
    [SerializeField] private GameObject pausedScreen;

    public Image audioButton;

    private float score, highScore;

    private bool countScore, gamePaused, achievedScore20 = false, achievedScore40 = false, achievedScore60 = false, achievedScore80 = false, achievedScore100 = false;

    public bool startScene = false;

    void Start()
    {
        if (!startScene)
        {
            audioButton = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().GetComponentInChildren<Image>();

            mapManager = GetComponent<MapManager>();

            score = 0;
            highScore = 0;

            CountScore(true);

            gamePaused = false;
            pausedScreen.SetActive(false);
        }
    }

    void Update()
    {
        if (!startScene)
        {
            if (countScore)
            {
                ManageScore();
                ScoreFading();
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
            {
                if (!gamePaused)
                    PauseGame();
                else
                    ResumeGame();
            }
        }
    }

    private void ManageScore()
    {
        score += Time.deltaTime;

        scoreText.text = score.ToString("N2");

        if (score > highScore)
            highScore = score;

        highScoreText.text = "HS: " + highScore.ToString("N2");
    }

    public float GetScore()
    {
        return score;
    }

    public void ScoreFading()
    {
        if (score > 20 && !achievedScore20)
        {
            BuildingsFade(backgroundParent.GetChild(5).transform, false);
            achievedScore20 = true;
        }
        else if (score > 40 && !achievedScore40)
        {
            BuildingsFade(backgroundParent.GetChild(4).transform, false);
            achievedScore40 = true;
        }
        else if (score > 60 && !achievedScore60)
        {
            BuildingsFade(backgroundParent.GetChild(3).transform, false);
            achievedScore60 = true;
        }
        else if (score > 80 && !achievedScore80)
        {
            BuildingsFade(backgroundParent.GetChild(2).transform, false);
            achievedScore80 = true;
        }
        else if (score > 100 && !achievedScore100)
        {
            BuildingsFade(backgroundParent.GetChild(1).transform, false);
            achievedScore100 = true;
        }
    }

    private void BuildingsFade(Transform parent, bool fadeIn)
    {
        foreach (Parallax child in parent.GetComponentsInChildren<Parallax>())
        {
            child.StartFade(fadeIn);
        }
    }

    public void SetScore(float scoreVal)
    {
        score = scoreVal;
    }

    public void CountScore(bool count)
    {
        countScore = count;
    }

    public void ResetGame()
    {
        score = 0;
        scoreText.text = score.ToString("N2");

        mapManager.ResetMap();

        for (int i = 1; i < 6; i++)
        {
            BuildingsFade(backgroundParent.GetChild(i), true);
        }

        achievedScore20 = false;
        achievedScore40 = false;
        achievedScore60 = false;
        achievedScore80 = false;
        achievedScore100 = false;
    }

    private void PauseGame()
    {
        gamePaused = true;

        pausedScreen.SetActive(true);

        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        gamePaused = false;

        pausedScreen.SetActive(false);

        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
