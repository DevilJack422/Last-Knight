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
    [Header("Item")]
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform spawnPoint;

    private static readonly int OpenHash = Animator.StringToHash("Open");

    private bool isOpened;
    private bool isItemSpawned;
    private Animator animator;

    public bool CanInteract => !isOpened;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        if (!CanInteract) return;

        isOpened = true;

        if (animator != null)
        {
            animator.SetTrigger(OpenHash);
        }
        else
        {
            Debug.LogError("ChestInteration: Animator none found!");
        }
    }

    public void SpawnItem()
    {
        if (isItemSpawned) return;

        isItemSpawned = true;

        if (itemPrefab == null)
        {
            Debug.LogWarning("ChestInteraction: Item prefab none store yet!");
            return;
        }

        Vector3 spawnPostion = spawnPoint != null ? spawnPoint.position : transform.position;

        Instantiate(itemPrefab, spawnPostion, Quaternion.identity);
    }
}
