using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    [Header("Music Properties")]
    [SerializeField] private GameObject unmutedMusicIcon;
    [SerializeField] private GameObject mutedMusicIcon;
    [SerializeField] private Slider musicSlider;

    [Header("SFX Properties")]
    [SerializeField] private GameObject unmutedSFXIcon;
    [SerializeField] private GameObject mutedSFXIcon;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        musicSlider.value = AudioManager.Instance.musicSource.volume;
        sfxSlider.value = AudioManager.Instance.sfxSource.volume;
        mutedMusicIcon.SetActive(AudioManager.Instance.musicMuted);
        unmutedMusicIcon.SetActive(!AudioManager.Instance.musicMuted);
        mutedSFXIcon.SetActive(AudioManager.Instance.sfxMuted);
        unmutedSFXIcon.SetActive(!AudioManager.Instance.sfxMuted);
    }

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
        mutedMusicIcon.SetActive(AudioManager.Instance.musicMuted);
        unmutedMusicIcon.SetActive(!AudioManager.Instance.musicMuted);
    }
    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
        mutedSFXIcon.SetActive(AudioManager.Instance.sfxMuted);
        unmutedSFXIcon.SetActive(!AudioManager.Instance.sfxMuted);
    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(sfxSlider.value);
    }

   

}
