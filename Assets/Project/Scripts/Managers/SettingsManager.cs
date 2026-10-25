using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] BoolGameEvent inverseYAxisChanged;
    public static SettingsManager Instance;

    // Estado actual de los ajustes en memoria
    [HideInInspector] public SettingsData data;

    private async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
            return;
        }
        data = await SaveSystem.LoadSettings();
    }

    // Seteo de las configuraciones generales
    private void Start()
    {
        // Gráficos
        ChangeGraphicsQuality(data.qualityIndex);
        ChangeFPSLimit(data.fps);
        ChangeScreenMode(data.screenModeIndex);

        // Sonido
        ChangeSFXVolume(data.sfxVolume);
        ChangeMusicVolume(data.musicVolume);
    }

    /* Métodos */
    // Guarda el nuevo valor del Dropdown y llama a la clase que gestiona la calidad gráfica
    public async void ChangeGraphicsQuality(int value)
    {
        if (data.qualityIndex != value)
        {
            GraphicsQualityManager.Instance.SetGraphicsQuality(value);
            data.qualityIndex = value;
            await SaveSystem.SaveSettings(data);
        }
    }

    // Guarda el nuevo valor del Slider y llama a la clase que gestiona el audio
    public async void ChangeSFXVolume(float value)
    {
        data.sfxVolume = value;
        AudioManager.Instance.SetSFXVolume(value);
        await SaveSystem.SaveSettings(data);
    }

    // Guarda el nuevo valor del Slider y llama a la clase que gestiona el audio
    public async void ChangeMusicVolume(float value)
    {
        data.musicVolume = value;
        AudioManager.Instance.SetMusicVolume(value);
        await SaveSystem.SaveSettings(data);
    }

    // Guarda el nuevo valor del Dropdown y llama a la clase que gestiona la calidad gráfica
    public async void ChangeFPSLimit(int value)
    {
        data.fps = value;
        GraphicsQualityManager.Instance.SetFPSLimit(value);
        await SaveSystem.SaveSettings(data);
    }

    // Guarda el nuevo valor del Dropdown y llama a la clase que gestiona la calidad gráfica
    public async void ChangeScreenMode(int value)
    {
        data.screenModeIndex = value;
        GraphicsQualityManager.Instance.SetScreenMode(value);
        await SaveSystem.SaveSettings(data);
    }

    // Invertir el eje Y del movimiento del jugador
    public void InverseYAxisFunction(bool evt)
    {
        inverseYAxisChanged.Raise(evt);
    }
}