using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class LobbyMainPanel : MonoBehaviour
    {
        [Header("Login Panel")]
        public GameObject LoginPanel;

        public InputField PlayerNameInput;

        public Button StartGameButton;
        
        public void Awake()
        {
            PlayerNameInput.text = "Player " + Random.Range(1000, 10000);
        }
        
        public void OnStartGameButtonClicked()
        {
            
        }
        
        public void SetActivePanel(string activePanel)
        {
            LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
        }
    }
}