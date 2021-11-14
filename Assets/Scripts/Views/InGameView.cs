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
        [SerializeField] private Image[] hearts;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text bulletsText;
        [SerializeField] private Text coinsText;
        public Text timerText;
        private Player _player => RoomController.Instance.localPlayer;
        public override void Initialize()
        {
            EventHub.bulletsChanged += UpdateBullets;
            EventHub.coinsChanged += UpdateCoins;
        }

        public override void Show()
        {
            base.Show();
            Clear();
            SoundManager.Instance.PlayMusic();
        }

        private void Update()
        {
            if(!RoomController.Instance.isGameStarted)
                return;
            
            UpdateScore(_player.score);
        }

        private void Clear()
        {
            scoreText.text = "0";
            coinsText.text = "0";
        }

        private void UpdateScore(float score)
        {
            scoreText.text = score.ToString("0");
        }

        public void AddHealth(int hp)
        {
            hearts[hp].gameObject.SetActive(true);
        }
        
        public void RemoveHealth(int hp)
        {
            hearts[hp].gameObject.SetActive(false);
        }

        private void UpdateBullets(int bullets)
        {
            bulletsText.text = bullets + "/30"; 
        }

        private void UpdateCoins(int coins)
        {
            coinsText.text = coins.ToString();
        }

        private void OnDestroy()
        {
            EventHub.scoreChanged -= UpdateScore;
            EventHub.bulletsChanged -= UpdateBullets;
        }
    }
}
