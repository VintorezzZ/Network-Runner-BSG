using System;
using System.Collections.Generic;
using Com.MyCompany.MyGame;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace Views
{
    public class InGameView : View
    {
        [SerializeField] private Text scoreText;
        [SerializeField] private Text hpText;
        [SerializeField] private Text bulletsText;
        private Player _player => GameManager.Instance.localPlayer;
        public override void Initialize()
        {
            EventHub.healthChanged += UpdateHealth;
            EventHub.bulletsChanged += UpdateBullets;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Update()
        {
            if(!RoomController.Instance.isGameStarted)
                return;
            
            UpdateScore(_player.score);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if(arg0.buildIndex == 1)
            {
                Clear();
            }        
        }

        private void Clear()
        {
            scoreText.text = "0";
            hpText.text = "0";
            bulletsText.text = "0";
        }

        private void UpdateScore(float score)
        {
            scoreText.text = score.ToString("0");
        }
        
        private void UpdateHealth(int hp)
        {
            hpText.text = hp.ToString();
        }
        
        private void UpdateBullets(int bullets)
        {
            bulletsText.text = bullets.ToString(); 
        }
        
        private void OnDestroy()
        {
            EventHub.scoreChanged -= UpdateScore;
            EventHub.healthChanged -= UpdateHealth;
            EventHub.bulletsChanged -= UpdateBullets;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
