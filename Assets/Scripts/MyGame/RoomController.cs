using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomController : MonoBehaviour
{

    public static RoomController instance = null;

    private PhotonView _photonView = null;

    private HashSet<string> _readyUsers = new HashSet<string>();

    private bool _canStartGame = false;
    private bool timerStarted;
    private double time;

    public PlayerController myPlayer;
    private void Awake()
    {
        instance = this;
        _photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _canStartGame = PhotonNetwork.CurrentRoom.Players.Count <= _readyUsers.Count;
        }

        if (timerStarted && PhotonNetwork.Time >= time)
        {
            myPlayer.canMove = true;
            timerStarted = false;
            time = 0;
            Debug.LogError("Time Out");

        }
    }

    private void OnDestroy()
    {
        instance = null;
    }


    public void StartGame()
    {
        _photonView.RPC("StartGameRpc", RpcTarget.All, PhotonNetwork.Time + 3f);
    }


    [PunRPC]
    private void StartGameRpc(double startTime)
    {
        timerStarted = true;
        time = startTime;
        
        Debug.LogError("Start Game RPC");
    }

    public void Ready(bool val)
    {
        _photonView.RPC("ReadyRpc", RpcTarget.All, PhotonNetwork.LocalPlayer.UserId, val);
    }


    [PunRPC]
    private void ReadyRpc(string id, bool val)
    {
        if (val)
        {
            _readyUsers.Add(id);
        }
        else
        {
            _readyUsers.Remove(id);
        }
    }


    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            GUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.Label($"{player.Value.NickName}  {player.Value.UserId}   ");

            bool isReady = _readyUsers.Contains(player.Value.UserId);
            string text = isReady ? "Unready" : "Ready";
            if (player.Value.UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                if (GUILayout.Button(text))
                {
                    Ready(!isReady);
                }
            }
            else
            {
                GUILayout.Label(text);
            }

            GUILayout.EndHorizontal();

        }

        if(PhotonNetwork.IsMasterClient && _canStartGame)
        {
            if (GUILayout.Button("Start Game"))
            {
                StartGame();
            }
        }  

        GUILayout.EndVertical();


        if (timerStarted)
        {
            GUILayout.Label((time - PhotonNetwork.Time).ToString());

        }

        GUILayout.EndHorizontal();

    }


}
