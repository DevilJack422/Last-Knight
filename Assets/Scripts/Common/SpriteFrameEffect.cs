using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFrameEffect : MonoBehaviour
{
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float framesPerSecond = 15f;

    private SpriteRenderer spriteRenderer;
    private float timer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (frames == null || frames.Length == 0)
        {
            Destroy(gameObject);
            return;
        }

        spriteRenderer.sprite = frames[0];
    }

    void Update()
    {
        timer += Time.deltaTime;
        int index = (int)(timer * framesPerSecond);

        if (index >= frames.Length)
        {
            Destroy(gameObject);
            return;
        }

        spriteRenderer.sprite = frames[index];
    }
}
