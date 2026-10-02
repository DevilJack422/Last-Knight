using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 25;
    [SerializeField] private float knockBackForce = 5f;

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (!other.gameObject.CompareTag("Player")) return;

        if (other.gameObject.TryGetComponent(out IDamageable target))
        {
            target.TakeDamage(damage, transform.position, knockBackForce);
        }
    }
}
