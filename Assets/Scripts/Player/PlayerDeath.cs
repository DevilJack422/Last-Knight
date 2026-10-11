using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private float reloadDelay = 1f;
    [SerializeField] private AudioClip deathSFX;

    private static readonly int DeathHash = Animator.StringToHash("Death");

    private Health health;
    private Animator animator;

    void Awake()
    {
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        health.Died += OnDied;        
    }

    void OnDisable() 
    {
        health.Died -= OnDied;       
    }

    private void OnDied()
    {
        animator.SetTrigger(DeathHash);
        AudioManager.Instance.PlaySfx(deathSFX);
        StartCoroutine(ReloadAfterDelay());
    }

    private IEnumerator ReloadAfterDelay()
    {
        yield return new WaitForSeconds(reloadDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
