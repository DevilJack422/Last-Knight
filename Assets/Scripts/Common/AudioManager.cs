using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}

    [SerializeField] private AudioSource sFXSource;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioSource musicSource;

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
    }

    void Start() 
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();   
    }

    public void PlaySfx(AudioClip clip, float volume = 1f)
    {
        sFXSource.PlayOneShot(clip, volume);
    }
}
