using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LootEntry
{
    public GameObject prefab;
    public int count = 1;
}

public class ChestInteraction : MonoBehaviour, IInteractable
{
    [Header("Sprite")]
    [SerializeField] private Sprite openedChestSprite;

    [Header("Item")]
    [SerializeField] private LootEntry[] loots;
    [SerializeField] private Transform spawnPoint; // null = chest position
    [SerializeField] private float launchForce = 5f;
    [SerializeField] private float spreadAngle = 60f; // Item pop out in -60 to +60 degree upward

    private bool isOpened;
    private string chestId;
    private SpriteRenderer  spriteRenderer;

    public bool CanInteract => !isOpened;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // ID = Chest_current scene name_chest postion(x,y) (Chest_Gameplay_6,7)
        chestId = "Chest_" + SceneManager.GetActiveScene().name + "_" + gameObject.transform.position.x + "," + gameObject.transform.position.y; 
        
        if (SaveSystem.IsChestOpened(chestId))
        {
            isOpened = true;
            ShowOpened();
        }
    }

    public void Interact()
    {
        if (isOpened) return;

        isOpened = true;

        ShowOpened();
        SpawnLoot();
        SaveSystem.MarkChestOpened(chestId);
    }

    private void ShowOpened()
    {
        if (spriteRenderer != null && openedChestSprite != null)
        {
            spriteRenderer.sprite = openedChestSprite;
        }
    }

    private void SpawnLoot()
    {
        int total = 0;
        foreach (LootEntry entry in loots)
        {
            if (entry.prefab != null) total += entry.count;
        }
        if (total == 0) return;

        Vector3 origin = spawnPoint != null ? spawnPoint.position : transform.position;

        int index = 0;
        foreach (LootEntry entry in loots)
        {
            if (entry.prefab == null) continue;

            for (int i = 0; i < entry.count; i++)
            {
                GameObject item = Instantiate(entry.prefab, origin, Quaternion.identity);

                if (item.TryGetComponent(out Rigidbody2D rb))
                {
                    // t run from 0 - 1 throught out items, only 1 can spawn (0.5);
                    float t = total > 1 ? (float)index / (total - 1) : 0.5f;
                    float angle = Mathf.Lerp(-spreadAngle, spreadAngle, t);

                    Vector2 direction = Quaternion.Euler(0f, 0f, angle) * Vector2.up;
                    rb.AddForce(direction * launchForce, ForceMode2D.Impulse);
                }

                index++;
            }
        }
    }

    [ContextMenu("Delete save (just for testing)")]
    private void ResetSave()
    {
        SaveSystem.DeleteSave();
        Debug.Log("Save deleted, Stop and Play game again to make chest reset");
    }
}
