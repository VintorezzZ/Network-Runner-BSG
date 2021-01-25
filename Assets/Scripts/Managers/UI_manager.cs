using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_manager : MonoBehaviour
{
    public static UI_manager instance;

    [SerializeField] public GameObject home;
    [SerializeField] private GameObject restart;
    [SerializeField] private GameObject whilePlay;
    [SerializeField] private Text bestScoreText;
    [SerializeField] private Text coinsWhilePlayText;
    [SerializeField] private GameObject awesomeSpritePrefab;
    [SerializeField] private List<Sprite> awesomeSprites;
    [SerializeField] private Text bulletsText;
    [SerializeField] private Text healthText;
    [SerializeField] private GameObject newBestScore;
    
    private GameManager gm;
    

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

        bulletsText = GameObject.Find("Bullets Text").GetComponent<Text>();
        healthText = GameObject.Find("hp Text").GetComponent<Text>();
    }

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void UpdateScore()
    {
        if(gm.isBestScore)
            newBestScore.SetActive(true);
        
        bestScoreText.text = PlayerPrefs.GetInt("Coins", 0).ToString();
    }

    public void UpdateBulletstext(int amount)
    {
        bulletsText.text = "Bullets:\n" + amount + "/30";
    }
    public void UpdateHealttext(int amount)
    {
        healthText.text = "HP:\n" + amount;
    }
    public void UpdateWhilePlayCoins()
    {
        coinsWhilePlayText.text = "" + (gm.score).ToString();
    }

    public void ActivateHomeUI()
    {
        home.SetActive(true);
        restart.SetActive(false);
        whilePlay.SetActive(false);
    }


    public void ActivateWhilePlayUI()
    {
        home.SetActive(false);
        restart.SetActive(false);
        whilePlay.SetActive(true);
        PlayerController.canMove = true;
    }


    public void ActivateGameOverUI()
    {
        restart.SetActive(true);
    }

    public IEnumerator ShowAwesomeText()
    {
        int index = Random.Range(0, awesomeSprites.Count);
        awesomeSpritePrefab.GetComponent<Image>().sprite = awesomeSprites[index];
        awesomeSpritePrefab.SetActive(true);
        yield return new WaitForSeconds(1f);
        awesomeSpritePrefab.SetActive(false);
    }
}
