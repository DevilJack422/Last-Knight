using UnityEngine;
using UnityEngine.SceneManagement;

public class CampfireInteraction : MonoBehaviour, IInteractable
{
    [Header("Save Point")]
    [SerializeField] private SavePanelUI savePanel;
    [SerializeField] private Transform respawnPoint;

    public bool CanInteract => savePanel != null && !savePanel.IsOpen;

    public void Interact()
    {
        if (!CanInteract) return;

        Vector3 position = respawnPoint != null ? respawnPoint.position : transform.position;

        string currentScene = SceneManager.GetActiveScene().name;

        savePanel.Open(position, currentScene);
    }
}
