using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager instance;

        [HideInInspector] public PlayerController localPlayerController;

        internal int score;
        internal bool isBestScore;
        private int _bestScore;
        
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;
        
        private void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }

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
                if (PlayerController.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 0f, 10f), Quaternion.identity, 0);

                    localPlayerController = playerPrefab.GetComponent<PlayerController>();
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }

            //Time.timeScale = 0;  // нельзя использовать в мультиплеере
            
            //OnHome();
        }

        #region Photon Callbacks


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }


        #endregion

        #region Public Methods


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        #endregion

        #region Private Methods


        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            //PhotonNetwork.LoadLevel("Room for 1");
        }


        #endregion
        
        #region Photon Callbacks


        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }


        #endregion

        public void OnHome()
        {
            UI_manager.instance.ActivateHomeUI();
        }

        public void OnStart()
        {
            UI_manager.instance.ActivateWhilePlayUI();
            Time.timeScale = 1;
            InvokeRepeating("AddContinouslyScore", 0, 0.5f);
        }

        public void OnGameOver()
        {
            StopAddingScore();
            CalculateScore();
            UI_manager.instance.UpdateScore();
            UI_manager.instance.ActivateGameOverUI();
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
            UI_manager.instance.UpdateWhilePlayCoins();
        }

        public void StopAddingScore()
        {
            CancelInvoke("AddContinouslyScore");
        }
    }
}
