using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverUIManager : MonoBehaviour
{
    [SerializeField] StringGameEvent loadLvlRequest;

    PanelRenderer panelRenderer;

    VisualTreeAsset _cachedVisualTreeAsset;

    Label _highscoreText, _highTimeText, _scoreText, _timeText;

    Button _restartButton, _exitButton;

    private void Awake() => panelRenderer = GetComponent<PanelRenderer>();
    

    private void OnEnable() => panelRenderer.RegisterUIReloadCallback(OnUIReload);
    

    private void OnDisable() => panelRenderer.UnregisterUIReloadCallback(OnUIReload);

    void OnUIReload(PanelRenderer panel, VisualElement root, int version)
    {
        _highscoreText = root.Q<Label>("highscore-text");
        _highscoreText.text = SettingsManager.Instance.data.highscores[0].ToString(); // Mostrar el record

        _highTimeText = root.Q<Label>("highTime-text");
        _highTimeText.text = TimeSpan.FromSeconds(SettingsManager.Instance.data.highTimes[0]).ToString(@"mm\:ss");

        _scoreText = root.Q<Label>("score-text");
        _scoreText.text = LevelManager.Instance.score.ToString(); // Mostrar la puntuación final y el tiempo jugado

        _timeText = root.Q<Label>("time-text");
        _timeText.text = TimeSpan.FromSeconds(LevelManager.Instance.lastWholeSecond).ToString(@"mm\:ss");

        _restartButton = root.Q<Button>("restart-button");
        _restartButton.RegisterCallback<ClickEvent>(RestartGame);

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
    }

    // Botón de reiniciar el juego
    public void RestartGame(ClickEvent evt)
    {
        loadLvlRequest.Raise(SceneManager.GetActiveScene().name);
    }

    // Salir de la partida y volver al menú principal
    public void Exit(ClickEvent evt) => loadLvlRequest.Raise("MainMenu");
}