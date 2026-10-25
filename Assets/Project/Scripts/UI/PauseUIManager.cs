using UnityEngine;
using UnityEngine.UIElements;

public class PauseUIManager : MonoBehaviour
{
    [SerializeField] GameEvent onSettingsRequest;

    PanelRenderer panelRenderer;

    private void Awake() => panelRenderer = GetComponent<PanelRenderer>();

    private void OnEnable() => panelRenderer.RegisterUIReloadCallback(OnUIReload);

    private void OnDisable() => panelRenderer.UnregisterUIReloadCallback(OnUIReload);

    void OnUIReload(PanelRenderer panel, VisualElement root, int version)
    {

    }

    public void Open() => panelRenderer.enabled = true;

    public void Close() => panelRenderer.enabled = false;

    // Menú de configuración desde el menú de pausa en partida
    public void BtnPauseSettings() => onSettingsRequest.Raise();
}