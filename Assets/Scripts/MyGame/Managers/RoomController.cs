using System;
using Cinemachine;
using Com.MyCompany.MyGame;
using UnityEngine;
using UnityEngine.UI;
using Utils;


public class RoomController : SingletonBehaviour<RoomController>
{
    private double _startGameDelay = 3;
    [SerializeField] private Text timerText;
    
    public Player myPlayer;
    private CinemachineVirtualCamera _camera;

    public Timer startTimer = new Timer();
    public bool isGameStarted = false;

    private void Awake()
    {
        InitializeSingleton();
    }

    public void Init(Player player)
    {
        myPlayer = player;

        _camera = FindObjectOfType<CinemachineVirtualCamera>();
        _camera.Follow = myPlayer.transform;
        _camera.LookAt = myPlayer.transform;
    }
    private void Update()
    {
        if(!startTimer.IsStarted)
            return;
        
        if (startTimer.IsStarted)
            UpdateTimerView();
        
        if (startTimer.Time >= _startGameDelay)
            OnTimerEnds();
    }

    private void UpdateTimerView()
    {
        double countdown = _startGameDelay - startTimer.Time;
        timerText.text = $"Game starts in {countdown:n0} seconds";
    }

    private void OnTimerEnds()
    {
        StartGame();
    }
    private void StartGame()
    {
        isGameStarted = true;
        startTimer.Stop();
        GameManager.Instance.StartGame();
        timerText.text = string.Empty;

        Debug.LogError("Start Game RPC");
    }
}
