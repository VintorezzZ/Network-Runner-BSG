using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitApp : MonoBehaviour
{
    private Button _exitButton;
    
    void Start()
    {
        _exitButton = GetComponent<Button>();
        _exitButton.onClick.AddListener(Quit);
    }

    private void Quit()
    {
        Application.Quit();
    }
}
