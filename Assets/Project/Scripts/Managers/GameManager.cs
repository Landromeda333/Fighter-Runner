using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameEvent inGameOnly, uIOnly;

    public static GameManager Instance;

    [HideInInspector, NonSerialized] public InGameState InGame;
    [HideInInspector, NonSerialized] public MainMenuState MainMenu;
    [HideInInspector, NonSerialized] public PreparationState Preparation;
    [HideInInspector, NonSerialized] public OnPauseState Pause;
    [HideInInspector, NonSerialized] public GameOverState GameOver;

    IGameState previousState;

    [HideInInspector] public IGameState currentState;

    GameStateMachine stateMachine = new GameStateMachine();

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
        Preparation = new PreparationState(inGameOnly);
        Pause = new OnPauseState(uIOnly);
        GameOver = new GameOverState(uIOnly);
    }

    private void Start() => currentState = MainMenu;

    public void OnPauseRequest() => SetGameState(Pause);

    public void OnContinueGame() => SetGameState(previousState);

    public void OnGoToMainMenu() => SetGameState(MainMenu);

    public void OnPreparation() => SetGameState(Preparation);

    public void OnPlaneAvailable() => SetGameState(InGame);

    public void OnGameOver() => SetGameState(GameOver);

    void SetGameState(IGameState state)
    {
        stateMachine.ChangeState(state);
        previousState = currentState;
        currentState = state;
    }
}