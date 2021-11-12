using MyGame.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Views;

namespace Com.MyCompany.MyGame
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        [HideInInspector] public Player localPlayer;
        
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;
        public float addScoreDelay = 3f;
        
        private void Awake()
        {
            InitializeSingleton();

            EventHub.gameOvered += OnGameOver;
        }

        private void Start()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
            }
            else
            {
                var player = Instantiate(playerPrefab, new Vector3(0f, 0f, 10f), Quaternion.identity);
                localPlayer = player.GetComponent<Player>();
                RoomController.Instance.Init(localPlayer);
            }
        }

        private void OnGameOver()
        {
            ViewManager.Show<GameOverView>();
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(0);
        }
        
        public void OnClick()
        {
            AudioManager.Instance.PlayClickSFX();
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void StartGame()
        {
            EventHub.OnGameStarted();
            ViewManager.Show<InGameView>();
            Time.timeScale = 1;
        }

        public void UnpauseGame()
        {
            
        }
    }
}
