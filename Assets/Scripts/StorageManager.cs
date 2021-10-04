using System;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    private bool _musicState;
    private bool _soundState;
    private float _musicVolume;
    private float _soundVolume;

    private static StorageManager _self;

    private void Awake()
    {
        _self = this;

        //MenuUIManager.OnMusicToggle += SetMusicState;
        //MenuUIManager.OnSoundToggle += SetSoundState;
        //MenuUIManager.OnMusicChange += SetMusicVolume;
        //MenuUIManager.OnSoundChange += SetSoundVolume;

        //UIManager.OnMusicToggle += SetMusicState;
        //UIManager.OnSoundToggle += SetSoundState;
        //UIManager.OnMusicChange += SetMusicVolume;
        //UIManager.OnSoundChange += SetSoundVolume;
    }

    public static event Action<bool> OnSetMusicState;
    public static event Action<bool> OnSetSoundState;
    public static event Action<float> OnSetMusicVolume;
    public static event Action<float> OnSetSoundVolume;

    void Start()
    {
        SetMusicState(PlayerPrefs.HasKey("music_on") ? Convert.ToBoolean(PlayerPrefs.GetInt("music_on")) : true);
        SetSoundState(PlayerPrefs.HasKey("sound_on") ? Convert.ToBoolean(PlayerPrefs.GetInt("sound_on")) : true);
        SetMusicVolume(PlayerPrefs.HasKey("music_volume") ? Mathf.Clamp(PlayerPrefs.GetFloat("music_volume"), 0, 100) : 50);
        SetSoundVolume(PlayerPrefs.HasKey("sound_volume") ? Mathf.Clamp(PlayerPrefs.GetFloat("sound_volume"), 0, 100) : 50);
    }

    private void SetMusicState(bool value)
    {
        _musicState = value;
        OnSetMusicState?.Invoke(_musicState);
        PlayerPrefs.SetInt("music_on", Convert.ToInt32(_musicState));
    }

    private void SetSoundState(bool value)
    {
        _soundState = value;
        OnSetSoundState?.Invoke(_soundState);
        PlayerPrefs.SetInt("sound_on", Convert.ToInt32(_soundState));
    }

    private void SetMusicVolume(float value)
    {
        _musicVolume = value;
        OnSetMusicVolume?.Invoke(_musicVolume);
        PlayerPrefs.SetFloat("music_volume", _musicVolume);
    }

    private void SetSoundVolume(float value)
    {
        _soundVolume = value;
        OnSetSoundVolume?.Invoke(_soundVolume);
        PlayerPrefs.SetFloat("sound_volume", _soundVolume);
    }

    public static bool GetSoundState()
    {
        return _self._soundState;
    }

    public static bool GetMusicState()
    {
        return _self._musicState;
    }

    public static float GetSoundVolume()
    {
        return _self._soundVolume;
    }

    public static float GetMusicVolume()
    {
        return _self._musicVolume;
    }

    private void OnDestroy()
    {
        //MenuUIManager.OnMusicToggle -= SetMusicState;
        //MenuUIManager.OnSoundToggle -= SetSoundState;
        //MenuUIManager.OnMusicChange -= SetMusicVolume;
        //MenuUIManager.OnSoundChange -= SetSoundVolume;

        //UIManager.OnMusicToggle -= SetMusicState;
        //UIManager.OnSoundToggle -= SetSoundState;
        //UIManager.OnMusicChange -= SetMusicVolume;
        //UIManager.OnSoundChange -= SetSoundVolume;
    }
}