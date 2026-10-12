using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}

    [Header("Sources")]
    [SerializeField] private AudioSource sFXSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Volume")]
    [Range(0f, 1f)] [SerializeField] private float musicVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float sFXVolume = 1f;

    private Coroutine fadeRoutine;

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        musicSource.loop = true;
        musicSource.volume = musicVolume;
    }

    public void PlaySfx(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null) return;
        sFXSource.PlayOneShot(clip, sFXVolume * volumeScale);
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1f)
    {
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(SwitchMusic(clip, fadeDuration));
    }

    public void StopMusic(float fadeDuration = 1f)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeOutAndStop(fadeDuration
        ));
    }

    private IEnumerator SwitchMusic(AudioClip newClip, float duration)
    {
        if (musicSource.isPlaying)
        {
            yield return FadeVolume(musicVolume, 0f, duration / 2f);
        }
        musicSource.clip = newClip;
        musicSource.Play();
    }

    private IEnumerator FadeOutAndStop(float duration)
    {
        yield return FadeVolume(0f, musicVolume, duration / 2f);
        musicSource.Stop();
    }

    private IEnumerator FadeVolume(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // use unscale to fade still playing when pause game (timeScale = 0)
            musicSource.volume = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        musicSource.volume = to;
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float value)
    {
        sFXVolume = Mathf.Clamp01(value);
    }
}
