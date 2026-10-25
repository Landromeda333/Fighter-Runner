using NUnit.Framework;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] GameEvent scoresReady;

    const int maxRecords = 10;

    public async void OnGameOver(int score, float timePlayed)
    {
        int ammount = SettingsManager.Instance.data.highscores.Count;
        SortHighscores(score);
        SortHighTimes(timePlayed);
        await SaveSystem.SaveSettings(SettingsManager.Instance.data);
        scoresReady.Raise();
    }

    void SortHighscores(int score)
    {
        for (int i = 0; i < SettingsManager.Instance.data.highscores.Count; i++)
        {
            // Actualización de record puntuación
            if (score > SettingsManager.Instance.data.highscores[i])
            {
                SettingsManager.Instance.data.highscores.Insert(i, score);
                if (SettingsManager.Instance.data.highscores.Count > maxRecords)
                {
                    SettingsManager.Instance.data.highscores.RemoveAt(SettingsManager.Instance.data.highscores.Count-1);
                }
                return; // Salir del bucle una vez se haya actualizado el record
            }
        }
        SettingsManager.Instance.data.highscores.Add(score);
        if (SettingsManager.Instance.data.highscores.Count > maxRecords)
        {
            SettingsManager.Instance.data.highscores.RemoveAt(SettingsManager.Instance.data.highscores.Count - 1);
        }
    }

    void SortHighTimes(float timePlayed)
    {
        for (int i = 0; i < SettingsManager.Instance.data.highTimes.Count; i++)
        {
            if (timePlayed > SettingsManager.Instance.data.highTimes[i])
            {
                SettingsManager.Instance.data.highTimes.Insert(i, timePlayed);
                if (SettingsManager.Instance.data.highTimes.Count > maxRecords)
                {
                    SettingsManager.Instance.data.highTimes.RemoveAt(SettingsManager.Instance.data.highTimes.Count-1);
                }
                return; // Salir del bucle una vez se haya actualizado el record
            }
        }
        SettingsManager.Instance.data.highTimes.Add(timePlayed);
        if (SettingsManager.Instance.data.highTimes.Count > maxRecords)
        {
            SettingsManager.Instance.data.highTimes.RemoveAt(SettingsManager.Instance.data.highTimes.Count - 1);
        }
    }
}