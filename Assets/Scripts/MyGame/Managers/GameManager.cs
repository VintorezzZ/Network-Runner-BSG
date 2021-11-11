using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Views;

namespace Com.MyCompany.MyGame
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        [HideInInspector] public PlayerController localPlayerController;

        internal int score;
        internal bool isBestScore;
        private int _bestScore;
        
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;
        
        private void Awake()
        {
            InitializeSingleton();
        }

        private void Start()
        {
            //PlayerPrefs.SetInt("Coins", 0);
            _bestScore = PlayerPrefs.GetInt("Coins", 0);
            score = 0;

            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
            }
            else
            {
                if (PlayerController.LocalPlayerInstance != null) 
                    return;

                var player = Instantiate(playerPrefab, new Vector3(0f, 0f, 10f), Quaternion.identity);
                localPlayerController = player.GetComponent<PlayerController>();
                RoomController.Instance.Init(localPlayerController);
            }

            //OnHome();
        }

        public void OnHome()
        {
            //UiManager.instance.ActivateHomeUI();
        }

        public void OnGameOver()
        {
            StopAddingScore();
            CalculateScore();

            ViewManager.Show<GameOverView>();
        }

        public void ReloadScene()
        {
            StopAddingScore();
            SceneManager.LoadScene(0);
        }

        public void CalculateScore()
        {
            if (score > _bestScore)
            {
                _bestScore = score;
                PlayerPrefs.SetInt("Coins", _bestScore);
                isBestScore = true;
            }
        }

        public void OnClick()
        {
            AudioManager.instance.PlayClickSFX();
        }

        public void AddContinouslyScore()
        {
            score += 3;
            EventHub.OnScoreChanged(score);
        }

        public void StopAddingScore()
        {
            CancelInvoke("AddContinouslyScore");
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
            ViewManager.Show<InGameView>();
            Time.timeScale = 1;
            InvokeRepeating("AddContinouslyScore", 0, 0.5f);
        }

        public void UnpauseGame()
        {
            
        }
    }
}
