using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager instance;

        [HideInInspector] public PlayerController playerController;

        internal int score;
        internal bool isBestScore;
        private int bestScore;


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
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
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

            playerController = FindObjectOfType<PlayerController>();
        }

        private void Start()
        {
            //PlayerPrefs.SetInt("Coins", 0);
            bestScore = PlayerPrefs.GetInt("Coins", 0);
            score = 0;

            Time.timeScale = 0;
            //OnHome();
        }

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
            if (score > bestScore)
            {
                bestScore = score;
                PlayerPrefs.SetInt("Coins", bestScore);
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
