using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

public class InfoPanelManager : MonoBehaviour
{
    [SerializeField] GameEvent onInfoClosed;

    PanelRenderer panelRenderer;

    VisualTreeAsset _cachedVisualTreeAsset;

    Button _exitButton;

    async void SetLocalizedText(Label label, string tableKey) => label.text = await new LocalizedString("UIText", tableKey).GetLocalizedStringAsync().Task;

    private void Awake() => panelRenderer = GetComponent<PanelRenderer>();

    private void OnEnable() => panelRenderer.RegisterUIReloadCallback(OnUIReload);

    private void OnDisable() => panelRenderer.UnregisterUIReloadCallback(OnUIReload);

    void OnUIReload(PanelRenderer panel, VisualElement root, int version)
    {
        SetLocalizedText(root.Q<Label>("pitch-text"), "info.pitch");
        SetLocalizedText(root.Q<Label>("warp-text"), "info.warp");
        SetLocalizedText(root.Q<Label>("yaw-text"), "info.yaw");
        SetLocalizedText(root.Q<Label>("shoot-text"), "info.shoot");
        SetLocalizedText(root.Q<Label>("missile-text"), "info.missile");
        SetLocalizedText(root.Q<Label>("warning-text"), "info.warning");

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
        onInfoClosed.Raise();
    }

    void Exit(ClickEvent evt) => Close();
}