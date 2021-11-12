// using System.Collections;
// using System.Collections.Generic;
// using Com.MyCompany.MyGame;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class UiManager : SingletonBehaviour<UiManager>
// {
//     [SerializeField] public GameObject home;
//     [SerializeField] private GameObject restart;
//     [SerializeField] private GameObject whilePlay;
//     [SerializeField] private Text bestScoreText;
//     [SerializeField] private GameObject awesomeSpritePrefab;
//     [SerializeField] private List<Sprite> awesomeSprites;
//     [SerializeField] private GameObject newBestScore;
//     
//     private GameManager gm;
//     
//
//     private void Awake()
//     {
//         InitializeSingleton();
//     }
//
//     private void Start()
//     {
//         gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
//     }
//
//     public void UpdateScore()
//     {
//         if(gm.isBestScore)
//             newBestScore.SetActive(true);
//         
//         bestScoreText.text = PlayerPrefs.GetInt("Coins", 0).ToString();
//     }
//
//     public void ActivateHomeUI()
//     {
//         restart.SetActive(false);
//         whilePlay.SetActive(false);
//         home.SetActive(true);
//     }
//
//
//     public void ActivateWhilePlayUI()
//     {
//         restart.SetActive(false);
//         whilePlay.SetActive(true);
//         //PlayerController.canMove = true;
//         home.SetActive(false);
//     }
//
//
//     public void ActivateGameOverUI()
//     {
//         restart.SetActive(true);
//     }
//
//     public IEnumerator ShowAwesomeText()
//     {
//         int index = Random.Range(0, awesomeSprites.Count);
//         awesomeSpritePrefab.GetComponent<Image>().sprite = awesomeSprites[index];
//         awesomeSpritePrefab.SetActive(true);
//         yield return new WaitForSeconds(1f);
//         awesomeSpritePrefab.SetActive(false);
//     }
// }
