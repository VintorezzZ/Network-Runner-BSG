using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public int score;
    public bool isBestScore;
    private int bestScore;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        //PlayerPrefs.SetInt("Coins", 0);
        bestScore = PlayerPrefs.GetInt("Coins",0);
        score = 0;

        Time.timeScale = 0;
        OnHome();
    }
    public void OnHome()
    {
        UI_manager.instance.ActivateHomeUI();
    }

    public void OnStart()
    {
        UI_manager.instance.ActivateWhilePlayUI();
        Time.timeScale = 1;
        InvokeRepeating("AddContinouslyScore", 0, 0.5f);
    }
    
    public void OnGameOver()
    {
        StopAddingScore();
        CalculateScore();
        UI_manager.instance.UpdateScore();
        UI_manager.instance.ActivateGameOverUI();
    }

    public void ReloadScene()
    {
        StopAddingScore();
        SceneManager.LoadScene(0);
    }

    public void CalculateScore()
    {
        if (score > bestScore) 
        {
            bestScore = score;
            PlayerPrefs.SetInt("Coins", bestScore);
            isBestScore = true;
        }
    }

    public void OnClick()
    {
        AudioManager.instance.PlayClickSFX();
    }

    public void AddContinouslyScore()
    {
        score += 3;
        UI_manager.instance.UpdateWhilePlayCoins();
    }

    public void StopAddingScore()
    {
        CancelInvoke("AddContinouslyScore");
    }
}
