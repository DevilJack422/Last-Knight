using UnityEngine;
using UnityEngine.Audio;

public class VolumeUI : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }
}
