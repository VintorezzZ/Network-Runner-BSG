using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

public class SoundManager : SingletonBehaviour<SoundManager>
{
    public static float CurrentVolume => AudioListener.volume;

    public float globalVolume = .5f;
    [SerializeField] AudioSource musicSource, uiSource, inGameSource;
    [SerializeField] AudioMixer uiMixer, musicMixer, inGameMixer;

    [SerializeField] AudioClip clickSfx;
    [SerializeField] AudioClip pickUpSfx;
    [SerializeField] AudioClip loseSfx;
    [SerializeField] AudioClip winSfx;
    [SerializeField] AudioClip boomSfx;
    [SerializeField] AudioClip fireSfx;
    [SerializeField] AudioClip music;
    [SerializeField] AudioClip hitSfx;
    [SerializeField] AudioClip coinPickUpSfx;

    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        
        InitializeSingleton();

        AudioListener.volume = globalVolume;
        inGameSource.outputAudioMixerGroup = inGameMixer.FindMatchingGroups("Master")[0];
        
        EventHub.gamePaused += OnGamePaused;
        EventHub.gameOvered += OnGameOvered;
    }

    private void OnGameOvered()
    {
        FadeMixerGroup(inGameMixer, CurrentVolume * .5f);
        FadeMixerGroup(musicMixer, CurrentVolume * .5f);
        PlayLose();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        EventHub.gamePaused -= OnGamePaused;
        EventHub.gameOvered -= OnGameOvered;
    }

    private void OnGamePaused(bool paused)
    {
        if(_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

        FadeMixerGroup(inGameMixer, paused ? globalVolume * .5f : globalVolume, duration: .5f);
    }

    private void FadeMixerGroup(AudioMixer audioMixer, float targetVolume, string exposedParam = "volume", float duration = 1f)
    {
        _fadeCoroutine = StartCoroutine(MixerGroupFader.StartFade(audioMixer, exposedParam, duration, targetVolume));
    }

    public void PlayBoom()
    {
        inGameSource.PlayOneShot(boomSfx);
    }
    
    public void PlayClick()
    {
        uiSource.PlayOneShot(clickSfx);
    }

    public void PlayLose() 
    {
        uiSource.PlayOneShot(loseSfx);
    }

    public void PlayWin()
    {
        uiSource.PlayOneShot(winSfx);
    }

    public void PlayPickUp()
    {
        uiSource.PlayOneShot(pickUpSfx);
    }
    
    public void PlayMusic()
    {
        musicSource.clip = music;
        musicSource.Play();
    }
    
    public void PlayHit()
    {
        uiSource.PlayOneShot(hitSfx);
    }
    
    public void PlayCoinPickUp()
    {
        uiSource.PlayOneShot(coinPickUpSfx);
    }
    
    public void PlayFire()
    {
        inGameSource.PlayOneShot(fireSfx);
    }
    
}
