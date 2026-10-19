using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private float pickupDelay = 0.5f;
    [SerializeField] private float pickupRadius = 0.5f;
    [SerializeField] private LayerMask playerLayer;

    private float pickupAllowTime;
    private bool isCollected;

    void Start()
    {
        pickupAllowTime = Time.time + pickupDelay;
    }

    void Update()
    {
        if (!isCollected || Time.time < pickupAllowTime) return;

        Collider2D hit = Physics2D.OverlapCircle(transform.position, pickupRadius, playerLayer); 
        if (hit == null) return;

        // Todo: Add use store item function here;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
