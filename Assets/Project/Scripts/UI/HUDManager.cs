using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class HUDManager : MonoBehaviour
{
    [SerializeField] GameEvent startGameRequest, onPreparation;

    PanelRenderer panelRenderer;

    Label _scoreText, _speedNumText, _timeText, _warningText;

    Button _startButton;

    Image _crosshire1, _crosshire2, _crosshire3;

    IVisualElementScheduledItem warningBlinking;

    //float blinkSpeed = 2f;

    private void Awake() => panelRenderer = GetComponent<PanelRenderer>();

    private void OnEnable() => panelRenderer.RegisterUIReloadCallback(OnUIReload);

    private void OnDisable() => panelRenderer.UnregisterUIReloadCallback(OnUIReload);

    private void Start() => onPreparation.Raise();

    void OnUIReload(PanelRenderer panel, VisualElement root, int version)
    {
        _scoreText = root.Q<Label>("score-text");
        _speedNumText = root.Q<Label>("speedNum-text");
        _timeText = root.Q<Label>("time-text");
        _warningText = root.Q<Label>("warning-text");

        _crosshire1 = root.Q<Image>("crosshair1-image");
        _crosshire2 = root.Q<Image>("crosshair2-image");
        _crosshire3 = root.Q<Image>("crosshair3-image");

        _startButton = root.Q<Button>("start-button");
        _startButton.RegisterCallback<ClickEvent>(StartGame);
    }

    public void Open() => panelRenderer.enabled = true;

    public void Close() => panelRenderer.enabled = false;

    // Empezar el juego
    public void StartGame(ClickEvent evt)
    {
        _startButton.style.display = DisplayStyle.None;
        startGameRequest.Raise();
    }

    public void UpdateScore(int score) => _scoreText.text = score.ToString();

    public void UpdateSpeed(int speed) => _speedNumText.text = speed.ToString() + " %";

    public void UpdateTime(int timeAmmount) => _timeText.text = TimeSpan.FromSeconds(timeAmmount).ToString(@"mm\:ss");

    public void OnPause() => Close();

    public void OnContinueGame() => Open();

    public void OnEndGameHandle() => Close();

    public void SwitchCameraHandle(int index)
    {
        switch (index)
        {
            case 0:
                _crosshire1.style.display = DisplayStyle.Flex;
                _crosshire2.style.display = DisplayStyle.None;
                _crosshire3.style.display = DisplayStyle.None;
                break;
            case 1:
                _crosshire1.style.display = DisplayStyle.None;
                _crosshire2.style.display = DisplayStyle.Flex;
                _crosshire3.style.display = DisplayStyle.None;
                break;
            case 2:
                _crosshire1.style.display = DisplayStyle.None;
                _crosshire2.style.display = DisplayStyle.None;
                _crosshire3.style.display = DisplayStyle.Flex;
                break;
        }
    }

    // Comienza el parapadeo del subtítulo
    public void StartBlink()
    {
        _warningText.style.display = DisplayStyle.Flex;
        warningBlinking = _warningText.schedule.Execute(() =>
        {
            _warningText.style.opacity = Mathf.PingPong(Time.time, 1);
        }).Every(16);
    }

    public void StopBlink()
    {
        warningBlinking?.Pause();
        _warningText.experimental.animation.Start(_warningText.style.opacity.value, 0, 1000, (e, val) =>
        {
            _warningText.style.opacity = val;
        }).Ease(Easing.OutCubic).OnCompleted(()=> _warningText.style.display = DisplayStyle.None);
    }
}