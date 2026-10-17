using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private SoundLibrary sFXLibrary;

    [SerializeField] private AudioSource sFXSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayClip(string groupId, string clipName)
    {
        if (Instance == null) return;

        AudioClip clip = sFXLibrary.GetClip(groupId, clipName);
        if (clip != null)
        {
            sFXSource.PlayOneShot(clip);
        }
    }
}
