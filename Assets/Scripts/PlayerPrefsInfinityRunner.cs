using UnityEngine;

public class PlayerPrefsInfinityRunner : MonoBehaviour
{
    void Awake()
    {
        if (!PlayerPrefs.HasKey("Initialized"))                 // Verifica si los PlayerPrefs ya han sido inicializados
        {
            for (int i = 0; i <= 9; i++)
            {
                PlayerPrefs.SetInt("highScore" + (i+1), 0);     // Records de puntuación
                PlayerPrefs.SetFloat("highTime" + (i + 1), 0);  // Records de tiempo
            }
            PlayerPrefs.SetInt("Initialized", 1);               // Marca como hecho
            PlayerPrefs.SetInt("inverseYAxis", 1);              // Estado del eje Y invertido
            PlayerPrefs.SetFloat("masterSound", 1);             // Estado del eje Y invertido
            PlayerPrefs.SetFloat("musicSound", 1);              // Volumen de los efectos
            PlayerPrefs.SetInt("musicMuted",0);                 // Estado del mute de la música
            PlayerPrefs.SetInt("masterMuted",0);                // Estado del mute del sonido maestro
            PlayerPrefs.Save();                                 // Se guardan los cambios en PlayerPrefs
        }
    }
}