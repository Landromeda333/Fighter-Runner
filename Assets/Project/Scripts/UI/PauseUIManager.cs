using UnityEngine;
using UnityEngine.UIElements;

public class PauseUIManager : MonoBehaviour
{
    [SerializeField] GameEvent onSettingsRequest;

    PanelRenderer panelRenderer;

    VisualTreeAsset _cachedVisualTreeAsset;

    private void Awake() => panelRenderer = GetComponent<PanelRenderer>();

    private void OnEnable() => panelRenderer.RegisterUIReloadCallback(OnUIReload);

    private void OnDisable() => panelRenderer.UnregisterUIReloadCallback(OnUIReload);

    void OnUIReload(PanelRenderer panel, VisualElement root, int version)
    {

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

    // Menú de configuración desde el menú de pausa en partida
    public void BtnPauseSettings() => onSettingsRequest.Raise();
}