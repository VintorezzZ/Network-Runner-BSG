using Com.MyCompany.MyGame;
using MyGame.Managers;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Views
{
    public class GameOverView : View
    {
        [SerializeField] private Button exitButton;
        [SerializeField] private Button restartButton;

        [SerializeField] private Text bestScoreText;
        
        public override void Initialize()
        {
            //exitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
            restartButton.onClick.AddListener(() =>
            {
                GameManager.Instance.RestartGame();
                Hide();
                ViewManager.Show<MainMenuView>();
            });

            EventHub.gameOvered += UpdateBestScore;
        }

        private void UpdateBestScore()
        {
            bestScoreText.text = PlayerPrefs.GetFloat("bestscore").ToString();
        }
    }
}
