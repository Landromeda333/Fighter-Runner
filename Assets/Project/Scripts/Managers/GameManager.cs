using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameEvent inGameOnly, uIOnly;

    static GameManager Instance;

    InGameState InGame;
    MainMenuState MainMenu;
    PreparationState Preparation;
    OnPauseState Pause;
    GameOverState GameOver;

    IGameState previousState, currentState;

    private GameStateMachine _stateMachine = new GameStateMachine();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
            return;
        }
        InGame = new InGameState(inGameOnly);
        MainMenu = new MainMenuState(uIOnly);
        Preparation = new PreparationState();
        Pause = new OnPauseState(uIOnly);
        GameOver = new GameOverState(uIOnly);
    }

    private void Start() => currentState = MainMenu;

    public void OnPauseRequest() => SetGameState(Pause);

    public void OnContinueGame() => SetGameState(previousState);

    public void OnGoToMainMenu() => SetGameState(MainMenu);

    public void SetGameState(IGameState state)
    {
        _stateMachine.ChangeState(state);
        previousState = currentState;
        currentState = state;
    }
}