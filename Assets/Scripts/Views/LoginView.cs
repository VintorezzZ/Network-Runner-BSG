using Com.MyCompany.MyGame;
using MyGame.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class LoginView : View
    {
        [SerializeField] private Button exitButton;
        [SerializeField] private Button loginButton;
        
        public InputField playerNameInput;
        
        public override void Initialize()
        {
            exitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
            loginButton.onClick.AddListener(() =>
            {
                PlayerPrefs.SetString("playername", playerNameInput.text);
                LoadMainMenu();
            });
        }

        private static void LoadMainMenu()
        {
            ViewManager.Show<MainMenuView>();
            WorldBuilder.Instance.Init(0);
        }

        public override void Show()
        {
            if(PlayerPrefs.HasKey("playername"))
            {
                LoadMainMenu();
                return;
            }
            
            base.Show();
            playerNameInput.text = "Player " + Random.Range(1000, 10000);
        }
    }
}