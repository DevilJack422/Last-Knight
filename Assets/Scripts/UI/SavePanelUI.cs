using System;
using TMPro;
using UnityEngine;

public class SavePanelUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text[] slotLabels;
    [SerializeField] private TMP_Text statusText;

    private Vector3 savePosition;
    private string sceneName;

    public bool IsOpen => panel != null && panel.activeSelf;

    void Awake()
    {
        if (panel != null) panel.SetActive(false);
    }

    void Update()
    {
        if (IsOpen && Input.GetKeyDown(KeyCode.Escape)) ClosePanel();
    }

    public void Open(Vector3 position, string currentScene)
    {
        savePosition = position;
        sceneName = currentScene;

        if (panel != null) panel.SetActive(true);
        if (statusText != null) statusText.text = "";

        RefreshSlot();
    }

    public void ClosePanel()
    {
        if (panel != null) panel.SetActive(false);
    }

    public void SaveSlot1() => SaveSlot(1);
    public void SaveSlot2() => SaveSlot(2);
    public void SaveSlot3() => SaveSlot(3);

    private void SaveSlot(int slot)
    {
        SaveData data = new SaveData
        {
            sceneName = sceneName,
            playerPostion = savePosition,
            saveAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
        };

        if (SaveSystem.Save(slot, data))
        {
            if (statusText != null) statusText.text = $"Saved to slot {slot}";
            RefreshSlot();
        }
        else
        {
            if (statusText != null) statusText.text = "Save failed!";
        }
    }

    public void RefreshSlot()
    {
        if (slotLabels == null) return;

        for (int i = 0; i < slotLabels.Length; i++)
        {
            if (slotLabels[i] == null) continue;

            int slot = i + 1;
            SaveData data = SaveSystem.Load(slot);

            if (data == null)
            {
                slotLabels[i].text = $"Slot {slot} - Empty";
            }
            else
            {
                slotLabels[i].text = $"Slot {slot}\n" +
                                    $"{data.sceneName}\n" +
                                    $"{data.saveAt}";
            }
        }
    }
}
