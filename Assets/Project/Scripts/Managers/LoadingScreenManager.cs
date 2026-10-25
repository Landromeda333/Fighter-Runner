using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadingScreenManager : MonoBehaviour
{
    PanelRenderer panelRenderer;

    VisualElement _backgroundImage;

    float timer = 0f;                           // Temporizador por si la escena tarda menos de lo previsto que le dé tiempo a la imagen a girar

    private void Awake()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        panelRenderer.enabled = true;
        panelRenderer.RegisterUIReloadCallback(OnUIReload);
    }

    private void OnDisable()
    {
        panelRenderer.UnregisterUIReloadCallback(OnUIReload);
        panelRenderer.enabled = false;
        _backgroundImage.style.opacity = 0;
    }

    void OnUIReload(PanelRenderer panel, VisualElement root, int version) => _backgroundImage = root.Q<VisualElement>("background-element");

    private void Update()
    {
        // Aparición del fondo de forma progresica
        if (_backgroundImage.style.opacity.value < 100)
            _backgroundImage.style.opacity = _backgroundImage.style.opacity.value + Time.unscaledDeltaTime;
    }

    public void LoadLevelHandle(string lvlName)
    {
        panelRenderer.enabled = true;
        enabled = true;
        StartCoroutine(LoadAsync(lvlName));
    }

    /* Corrutinas */
    // Carga del nivel
    public IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);// Carga la escena de forma asíncrona
        operation.allowSceneActivation = false;                           // No activa la escena hasta que esté cargada

        while (!operation.isDone)                                         // Mientras no esté cargada
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);    // Progreso de la carga
            timer += Time.unscaledDeltaTime;
            if (progress >= 1f && timer >= 5)                             // Si está cargada
            {
                operation.allowSceneActivation = true;                    // Activa la escena
            }
            yield return null;                                            // Sale del bucle
        }
        enabled = false;
    }
}