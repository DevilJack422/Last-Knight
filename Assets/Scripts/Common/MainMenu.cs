using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Panel Control")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionPanel;


    void Awake()
    {
        mainPanel.SetActive(true);
        optionPanel.SetActive(false);

        LoadVolume();

    }

    public void StartButton()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void OptionButton()
    {
        mainPanel.SetActive(false);
        optionPanel.SetActive(true);
    }

    public void BackButton()
    {
        optionPanel.SetActive(true == false);
        mainPanel.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SaveVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        audioMixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }
}
