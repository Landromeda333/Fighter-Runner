using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class SaveSystem
{
    // Ruta donde se guarda el archivo
    static string path = Path.Combine(Application.persistentDataPath, "settings.json");

    // Guarda los ajustes en un archivo JSON
    public static async Task SaveSettings(SettingsData data)
    {
        await File.WriteAllTextAsync(path, JsonUtility.ToJson(data, true));
    }

    // Carga los ajustes desde el archivo JSON
    public static async Task<SettingsData> LoadSettings()
    {
        if (File.Exists(path))
        {
            return JsonUtility.FromJson<SettingsData>(await File.ReadAllTextAsync(path));
        }
        else
        {
            SettingsData data = new SettingsData
            {
                qualityIndex = 2,
                fps = 60,
                screenModeIndex = 0,
                musicVolume = 80,
                sfxVolume = 80,
                inverseYAxis = false,
                highscores = new List<int>(),
                highTimes = new List<float>()
            };
            await SaveSettings(data);
            return data;
        }
    }
}