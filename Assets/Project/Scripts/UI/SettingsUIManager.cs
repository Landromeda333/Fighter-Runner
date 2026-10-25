using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsUIManager : MonoBehaviour
{
    [SerializeField] GameEvent settingsClosed;

    PanelRenderer panelRenderer;

    VisualTreeAsset _cachedVisualTreeAsset;

    DropdownField _qualityDrop, _fpsDrop, _screenDrop;

    Slider _musicSlider, _sfxSlider;

    Toggle _inverseYToggle;

    Button _exitButton;

    Dictionary<string, int> fpsOptions = new() { { "30", 30 }, { "60", 60 }}; // Diccionario con las opciones para los límites de FPS

    private void Awake() => panelRenderer = GetComponent<PanelRenderer>();

    private void OnEnable() => panelRenderer.RegisterUIReloadCallback(OnUIReload);

    private void OnDisable() => panelRenderer.UnregisterUIReloadCallback(OnUIReload);

    void OnUIReload(PanelRenderer panel, VisualElement root, int version)
    {
        _qualityDrop = root.Q<DropdownField>("quality-drop");
        _qualityDrop.RegisterValueChangedCallback(OnQualityChanged);

        _fpsDrop = root.Q<DropdownField>("fps-drop");
        _fpsDrop.RegisterValueChangedCallback(OnFPSChanged);

        _screenDrop = root.Q<DropdownField>("screen-drop");
        _screenDrop.RegisterValueChangedCallback(OnScreenModeChanged);

        _musicSlider = root.Q<Slider>("music-slider");
        _musicSlider.RegisterValueChangedCallback(OnMusicVolumeChanged);

        _sfxSlider = root.Q<Slider>("sfx-slider");
        _sfxSlider.RegisterValueChangedCallback(OnSFXVolumeChanged);

        _inverseYToggle = root.Q<Toggle>("inverse-toggle");
        _inverseYToggle.RegisterValueChangedCallback(OnInverseYAxis);

        _exitButton = root.Q<Button>("exit-button");
        _exitButton.RegisterCallback<ClickEvent>(Exit);
    }

    public void Open()
    {
        if (panelRenderer.TryGetComponent<UIDocument>(out UIDocument doc))
        {
            if (doc.visualTreeAsset == null)
                doc.visualTreeAsset = _cachedVisualTreeAsset;
        }
        panelRenderer.enabled = true;
    }

    public void Close()
    {
        panelRenderer.enabled = false;
        if (panelRenderer.TryGetComponent<UIDocument>(out UIDocument doc))
        {
            _cachedVisualTreeAsset = doc.visualTreeAsset;
            doc.visualTreeAsset = null;
        }
        settingsClosed.Raise();
    }

    // Volver al menú de pausa desde la sección de configuración
    public void Exit(ClickEvent evt) => Close();

    // FPS Cambiados
    void OnQualityChanged(ChangeEvent<string> evt) => SettingsManager.Instance.ChangeGraphicsQuality(_qualityDrop.index);

    // FPS Cambiados
    void OnFPSChanged(ChangeEvent<string> evt) => SettingsManager.Instance.ChangeFPSLimit(fpsOptions.TryGetValue(evt.newValue, out int fps) ? fps : -1);

    // Modo de pantalla cambiado
    void OnScreenModeChanged(ChangeEvent<string> evt) => SettingsManager.Instance.ChangeScreenMode(_screenDrop.index);

    // Volumen de la música cambiado
    void OnMusicVolumeChanged(ChangeEvent<float> evt) => SettingsManager.Instance.ChangeMusicVolume(evt.newValue);

    // Volumen de los efectos especiales cambiado
    void OnSFXVolumeChanged(ChangeEvent<float> evt) => SettingsManager.Instance.ChangeSFXVolume(evt.newValue);

    void OnInverseYAxis(ChangeEvent<bool> evt) => SettingsManager.Instance.InverseYAxisFunction(evt.newValue);
}