using System;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDManager : MonoBehaviour
{
    [SerializeField] GameEvent startGameRequest;

    PanelRenderer panelRenderer;

    VisualTreeAsset _cachedVisualTreeAsset;

    Label _scoreText, _speedNumText, _timeText;

    Button _startButton;

    Image _crosshire1, _crosshire2, _crosshire3;

    //float blinkSpeed = 2f;

    private void Awake() => panelRenderer = GetComponent<PanelRenderer>();

    private void OnEnable() => panelRenderer.RegisterUIReloadCallback(OnUIReload);

    private void OnDisable() => panelRenderer.UnregisterUIReloadCallback(OnUIReload);

    void OnUIReload(PanelRenderer panel, VisualElement root, int version)
    {
        _scoreText = root.Q<Label>("score-text");
        _speedNumText = root.Q<Label>("speedNum-text");
        _timeText = root.Q<Label>("time-text");

        _crosshire1 = root.Q<Image>("crosshair1-image");
        _crosshire2 = root.Q<Image>("crosshair2-image");
        _crosshire3 = root.Q<Image>("crosshair3-image");

        _startButton = root.Q<Button>("start-button");
        _startButton.RegisterCallback<ClickEvent>(StartGame);
    }

    /*
    private void Update()
    {
        
        if (warningPullUp.enabled)
        {
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            warningPullUp.color = new Color(warningPullUp.color.r, warningPullUp.color.g, warningPullUp.color.b, alpha);
        }
    }
    */

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
}