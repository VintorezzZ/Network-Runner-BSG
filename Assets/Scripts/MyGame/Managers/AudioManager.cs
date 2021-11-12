using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class AudioManager : SingletonBehaviour<AudioManager>
{
    public bool globalMute;
    public bool pauseMute;

    private AudioSource audioSource;
    public AudioClip clickSFX;
    public AudioClip pickUpSFX;
    public AudioClip loseSFX;
    public AudioClip winSFX;
    public AudioClip[] transitionsSFX;


    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        InitializeSingleton();
        
        EventHub.gameOvered += OnGameOvered;
    }

    private void OnGameOvered()
    {
        PlayLoseSFX();
    }

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void MutePlayAllSounds()
    {
        if (globalMute == true)
        {
            AudioListener.volume = 0;
            return;
        }
        else
        {
            if (pauseMute)
                AudioListener.volume = 0;
            else
                AudioListener.volume = 1;
        }
    }

    public void PlayClickSFX()
    {
        audioSource.PlayOneShot(clickSFX);
    }

    public void PlayLoseSFX() 
    {
        audioSource.PlayOneShot(loseSFX);
    }

    public void PlayWinSFX()
    {
        audioSource.PlayOneShot(winSFX);
    }

    public void PlayPickUpSFX()
    {
        audioSource.PlayOneShot(pickUpSFX);
    }

    public void PlayTransitionSFX()
    {
        int i = Random.Range(0, transitionsSFX.Length);
        audioSource.PlayOneShot(transitionsSFX[i]);
    }

    private void OnDestroy()
    {
        EventHub.gameOvered -= OnGameOvered;
    }
}
