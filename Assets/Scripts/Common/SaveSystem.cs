using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<string> openedChest = new List<string>();
}

public static class SaveSystem
{
    private static SaveData data;
    private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    private static SaveData Data
    {
        get
        {
            if (data == null) Load();
            return data;
        }
    }

    public static bool IsChestOpened(string id)
    {
        return Data.openedChest.Contains(id);
    }

    public static void MarkChestOpened(string id)
    {
        if (Data.openedChest.Contains(id)) return;

        Data.openedChest.Add(id);
        Save();
    }

    public static void Save()
    {
        File.WriteAllText(SavePath, JsonUtility.ToJson(Data, true));
    }

    public static void Load()
    {
        Debug.Log($"SaveSystem: file save locate in {SavePath}");

        if (File.Exists(SavePath))
        {
            try
            {
                data = JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath));
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"SaveSystem: file save error, create new. ({e.Message})");
            }
        }
    }

    public static void DeleteSave()
    {
        data = new SaveData();
        if (File.Exists(SavePath)) File.Delete(SavePath);
    }
}
