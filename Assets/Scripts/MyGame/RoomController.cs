using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class RoomController : MonoBehaviour
{

    public static RoomController instance = null;

    private PhotonView _photonView = null;

    private HashSet<string> _readyUsers = new HashSet<string>();

    private bool _canStartGame = false;
    private bool timerStarted;
    private double time;

    public PlayerController myPlayer;

    private CinemachineVirtualCamera _camera;
    private void Awake()
    {
        instance = this;
        _photonView = GetComponent<PhotonView>();

        _camera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        _camera.Follow = myPlayer.transform;
        _camera.LookAt = myPlayer.transform;
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
        _photonView.RPC("StartGameRpc", RpcTarget.All, PhotonNetwork.Time + 3f, Random.Range(0, 101));
    }


    [PunRPC]
    private void StartGameRpc(double startTime, int randomSeed)
    {
        timerStarted = true;
        time = startTime;

        WorldBuilder.instance.Seed = randomSeed;
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
            if (player.Value.UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                string text = isReady ? "Unready" : "Ready";

                if (GUILayout.Button(text))
                {

                    Ready(!isReady);
                }
            }
            else
            {
                string text = isReady ? "Ready" : "Unready";

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
