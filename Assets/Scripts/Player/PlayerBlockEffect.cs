using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerBlock))]
public class PlayerBlockEffect : MonoBehaviour
{
    [SerializeField] private GameObject flashPrefab;
    [Tooltip("Flash postion compare with Player when facing Right (Auto flip when look Left)")]
    [SerializeField] private Vector2 offset = new Vector2(0.6f, 0.2f);

    private PlayerBlock block;
    private PlayerMovement movement;

    void Awake()
    {
        block = GetComponent<PlayerBlock>();
        movement = GetComponent<PlayerMovement>();
    }

    void OnEnable()
    {
        block.Blocked += OnBlocked;
    }

    void OnDisable()
    {
        block.Blocked -= OnBlocked;
    }

    private void OnBlocked()
    {
        if (flashPrefab == null) return;

        int facing = movement.FacingDirection;
        Vector3 position = transform.position + new Vector3(offset.x * facing, offset.y, 0f);

        GameObject flash = Instantiate(flashPrefab, position, Quaternion.identity);

        if (flash.TryGetComponent(out SpriteRenderer sr))
        {
            sr.flipX = facing < 0;
        }
    }
}
