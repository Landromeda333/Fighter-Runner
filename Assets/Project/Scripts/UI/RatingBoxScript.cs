using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public struct ExampleData
{
    public int place, score;
    public float finalTime;
}

public class RatingBoxScript : MonoBehaviour
{
    /* Sprites */
    [SerializeField] Sprite rightArrow, leftArrow;                                            // Sprite flecha hacia la derecha

    [SerializeField] Sprite rightArrowPressed, leftArrowPressed;

    Button _ratingButton;

    PanelRenderer panelRenderer;

    Image _highscoresPanel;

    private void Awake() => panelRenderer = GetComponentInParent<PanelRenderer>();

    private void OnEnable() => panelRenderer.RegisterUIReloadCallback(OnUIReload);

    private void OnDisable() => panelRenderer.UnregisterUIReloadCallback(OnUIReload);

    public void Open() => _highscoresPanel.style.display = DisplayStyle.Flex;

    public void Close() => _highscoresPanel.style.display = DisplayStyle.None;

    void OnUIReload(PanelRenderer panel, VisualElement root, int version)
    {
        _highscoresPanel = root.Q<Image>("highscores-panel");

        _ratingButton = root.Q<Button>("highscore-button");
        _ratingButton.RegisterCallback<ClickEvent>(ShowHide);

        var listView = root.Q<MultiColumnListView>("highscores-table");

        List<ExampleData> highscores = new List<ExampleData>();
        for (int i = 0; i < SettingsManager.Instance.data.highscores.Count; i++)
        {
            highscores.Add(new ExampleData {place = i+1, score = SettingsManager.Instance.data.highscores[i], finalTime = SettingsManager.Instance.data.highTimes[i]});
        }

        // Creación de las filas
        listView.itemsSource = highscores;

        // Configuración de las columnas
        var colPlace = listView.columns["Position"];
        colPlace.makeCell = () => new Label();
        colPlace.bindCell = (element, index) =>
        {
            (element as Label).text = highscores[index].place.ToString();
        };

        var colScore = listView.columns["Score"];
        colScore.makeCell = () => new Label();
        colScore.bindCell = (element, index) =>
        {
            (element as Label).text = highscores[index].score.ToString();
        };

        var colTime = listView.columns["Time"];
        colTime.makeCell = () => new Label();
        colTime.bindCell = (element, index) =>
        {
            (element as Label).text = highscores[index].finalTime.ToString();
        };
    }

    /* Métodos */
    void ShowHide(ClickEvent evt) => _highscoresPanel.style.left = _highscoresPanel.style.left == Length.Percent(-34) ? Length.Percent(-0.3f) : Length.Percent(-34);
}