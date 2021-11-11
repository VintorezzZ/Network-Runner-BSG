using Com.MyCompany.MyGame;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class LoginView : View
    {
        [SerializeField] private Button exitButton;
        [SerializeField] private Button loginButton;
        public override void Initialize()
        {
            exitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
            loginButton.onClick.AddListener(() =>
            {
                ViewManager.Show<MainMenuView>();
            });
        }
    }
}
