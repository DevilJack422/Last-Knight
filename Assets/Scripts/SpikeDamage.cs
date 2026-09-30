using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    [Header("Damage Setting")]
    [SerializeField] private int damage = 25;
    [SerializeField] private float knockBackForce = 5f;

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage, transform.position, knockBackForce);
            }
        }   
    }
}
