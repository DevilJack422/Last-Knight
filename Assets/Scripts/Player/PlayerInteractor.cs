using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private GameObject interactionIcon;

    private readonly List<IInteractable> inRange = new List<IInteractable>();

    private Health health;

    void Awake()
    {
        health = GetComponent<Health>();

        SetIcon(false);
    }

    void Update()
    {
        if (health.IsDead || health == null)
        {
            SetIcon(false);
            return;
        }

        IInteractable target = GetTarget();

        SetIcon(target != null);

        if (target != null && Input.GetKeyDown(KeyCode.E))
        {
            target.Interact();
        }
    }

    void OnDisable()
    {
        inRange.Clear();
        SetIcon(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null && !inRange.Contains(interactable))
        {
            inRange.Add(interactable);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            inRange.Remove(interactable);
        }
    }

    private IInteractable GetTarget()
    {
        for (int i = inRange.Count - 1; i >= 0; i--)
        {
            IInteractable interactable = inRange[i];

            if (interactable == null)
            {
                inRange.RemoveAt(i);
                continue;
            }

            if (interactable.CanInteract) return interactable;
        }
        return null;
    }

    private void SetIcon(bool visible)
    {
        if (interactionIcon != null)
        {
            interactionIcon.SetActive(visible);
        }
    }
}
