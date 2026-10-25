using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] StringGameEvent loadLvlRequest;
    [SerializeField] GameEvent settingsRequest, onInfoRequest;
    [SerializeField] GameEvent onMainMenuLoaded;

    PanelRenderer panelRenderer;

    VisualTreeAsset _cachedVisualTreeAsset;

    Button _playButton, _infoButton, _settingsButton, _exitGameButton;

    async void SetLocalizedText(Button butt, string tableKey) => butt.text = await new LocalizedString("UIText", tableKey).GetLocalizedStringAsync().Task;

    private void Awake() => panelRenderer = GetComponent<PanelRenderer>();

    private void OnEnable() => panelRenderer.RegisterUIReloadCallback(OnUIReload);

    private void OnDisable() => panelRenderer.RegisterUIReloadCallback(OnUIReload);

    private void Start() => onMainMenuLoaded.Raise();

    void OnUIReload(PanelRenderer panel, VisualElement root, int version)
    {
        _playButton = root.Q<Button>("play-button");
        _playButton.RegisterCallback<ClickEvent>(PlayGame);
        SetLocalizedText(_playButton, "menu.play");

        _infoButton = root.Q<Button>("info-button");
        _infoButton.RegisterCallback<ClickEvent>(OnInfoPerformed);
        SetLocalizedText(_infoButton, "menu.info");

        _settingsButton = root.Q<Button>("settings-button");
        _settingsButton.RegisterCallback<ClickEvent>(OnSettingsPerformed);

        _exitGameButton = root.Q<Button>("exit-button");
        _exitGameButton.RegisterCallback<ClickEvent>(ExitGame);
        SetLocalizedText(_exitGameButton, "menu.exit");
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
    }

    void OnSettingsPerformed(ClickEvent evt)
    {
        _settingsButton.style.display = DisplayStyle.None;
        settingsRequest.Raise();
    }

    public void OnInfoPerformed(ClickEvent evt)
    {
        _settingsButton.style.display = DisplayStyle.None;
        onInfoRequest.Raise();
    }

    public void RestoreUI() => _settingsButton.style.display = DisplayStyle.Flex;

    void PlayGame(ClickEvent evt) => loadLvlRequest.Raise("Lvl1");

    void ExitGame(ClickEvent evt) => Application.Quit();
}