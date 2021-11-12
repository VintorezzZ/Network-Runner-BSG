using System.Collections;
using System.Collections.Generic;
using Com.MyCompany.MyGame;
using MyGame.Managers;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class MainView : View
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
        
    public override void Initialize()
    {
        exitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
        settingsButton.onClick.AddListener(() => ViewManager.Show<SettingsView>());
        startButton.onClick.AddListener(() =>
        {
            RoomController.Instance.startTimer.Start();
            Hide();
            ViewManager.Show<InGameView>();
        });
    }
}
