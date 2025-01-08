using Assets.Scripts;
using UnityEngine;

public static class SaveSystem
{
    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(GetSaveFilePath(), json);
        Debug.Log("Game data saved successfully.");
    }

    public static GameData Load()
    {
        string path = GetSaveFilePath();

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            return JsonUtility.FromJson<GameData>(json);
        }

        return null; // If no file exists, return null
    }

    private static string GetSaveFilePath()
    {
        return Application.persistentDataPath + "/GameData.json";
    }
}
