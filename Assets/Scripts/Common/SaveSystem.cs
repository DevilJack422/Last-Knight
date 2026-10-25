using System;
using System.IO;
using UnityEditor.PackageManager;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private const int MaxSaveSlot = 3;

    private static string GetPath(int slot)
    {
        if (slot < 1 || slot > MaxSaveSlot)
        {
            throw new ArgumentOutOfRangeException(nameof(slot));
        }

        return Path.Combine(Application.persistentDataPath, $"save_slot_{slot}.json");
    }

    public static bool HasSave(int slot) => File.Exists(GetPath(slot));

    public static SaveData Load(int slot)
    {
        string path = GetPath(slot);

        if (!File.Exists(path))
        {
            return null;
        }

        try
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        } 
        catch (Exception e)
        {
            Debug.LogError($"Failed to load slot {slot}: {e.Message}");
            return null;
        }
    }

    public static bool Save(int slot, SaveData data)
    {
        if (data == null) return false;

        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(GetPath(slot), json);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save slot {slot}: {e.Message}");
            return false;
        }
    }
}
