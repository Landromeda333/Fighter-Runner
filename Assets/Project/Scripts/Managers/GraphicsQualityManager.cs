using UnityEngine;

public class GraphicsQualityManager : MonoBehaviour
{
    /* Singleton */
    public static GraphicsQualityManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    /* Métodos */
    // Aplicación de la nueva calidad gráfica
    public void SetGraphicsQuality(int value)
    {
        QualitySettings.SetQualityLevel(value, true);
    }

    // Aplicación del límite de FPS
    public void SetFPSLimit(int value)
    {
        Application.targetFrameRate = value;
    }

    // Aplicación del nuevo modo de la pantalla
    public void SetScreenMode(int value)
    {
        switch (value)
        {
            case 0: Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen; break;
            case 1: Screen.fullScreenMode = FullScreenMode.FullScreenWindow; break;
            case 2: Screen.fullScreenMode = FullScreenMode.Windowed; break;
        }
    }
}