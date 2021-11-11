using Com.MyCompany.MyGame;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class GameOverView : View
    {
        [SerializeField] private Button exitButton;
        [SerializeField] private Button restartButton;
        
        public override void Initialize()
        {
            //exitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
            restartButton.onClick.AddListener(() =>
            {
                GameManager.Instance.RestartGame();
                Hide();
                ViewManager.Show<MainMenuView>();
            });
        }
    }
}
